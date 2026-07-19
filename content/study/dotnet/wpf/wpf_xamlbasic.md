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

##<a id="sec-generated-title-1"></a> <a id="structure"></a>基本構造
XAML の基本を説明するために、
「[XAML 概要（WPF）](wpf_xaml.md)」で例に出した、
テキストボックスを2つ表示するコードをもう1度見てみましょう。


<pre class="xsource" title="XAML でテキストボックス2つを表示">
<code><span class="bracket">&lt;</span><span class="element">Page</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Background</span><span class="attvalue">="White"</span>
  <span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">WrapPanel</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBox</span> 
      <span class="attribute">Width</span> <span class="attvalue">= "100"</span> <span class="attribute">FontSize</span> <span class="attvalue">= "30"</span> <span class="attribute">Text</span> <span class="attvalue">= "text 1"</span>
      <span class="attribute">Background</span> <span class="attvalue">= "White"</span> <span class="attribute">Foreground</span> <span class="attvalue">= "Blue"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBox</span> 
      <span class="attribute">Width</span> <span class="attvalue">= "100"</span> <span class="attribute">FontSize</span> <span class="attvalue">= "30"</span> <span class="attribute">Text</span> <span class="attvalue">= "text 2"</span>
      <span class="attribute">Background</span> <span class="attvalue">= "White"</span> <span class="attribute">Foreground</span> <span class="attvalue">= "Green"</span> /<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">Page</span><span class="bracket">&gt;</span>
</code></pre>
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

<pre class="source" title="" lang="">
<code><span class="reserved">class</span> Page1 : Page
{
  <span class="reserved">public</span> Page1()
  {
    <span class="reserved">this</span>.Background = Colors.White;

    WrapPanel panel1 = <span class="reserved">new</span> WrapPanel();

    <span class="reserved">this</span>.Content = panel1;

    TextBox textbox1 = <span class="reserved">new</span> TextBox();
    textbox1.Width = 100;
    textbox1.FontSize = 30;
    textbox1.Background = Colors.White;
    textbox1.Foreground = Colors.Blue;
    textbox1.Text = <span class="literal">"text 1"</span>;

    panel1.Children.Add(textbox1);

    TextBox textbox2 = <span class="reserved">new</span> TextBox();
    textbox2.Width = 100;
    textbox2.FontSize = 30;
    textbox2.Background = Colors.White;
    textbox2.Foreground = Colors.Green;
    textbox2.Text = <span class="literal">"text 2"</span>;

    panel1.Children.Add(textbox2);
  }
}
</code></pre>



##<a id="sec-generated-title-2"></a> <a id="property"></a>プロパティの設定
これまでにも何度か例に出ていますが、
XAML では、XML の属性として「[プロパティ](../../csharp/oop/oo_property.md#property)」の値を設定できます。


<pre class="xsource" title="XML 属性としてプロパティの値を設定">
<code><span class="bracket">&lt;</span><span class="element">TextBox</span> 
  <em><span class="attribute">Width</span> <span class="attvalue">= "100"</span> <span class="attribute">FontSize</span> <span class="attvalue">= "30"</span> <span class="attribute">Text</span> <span class="attvalue">= "text 1"</span></em>
  <em><span class="attribute">Background</span> <span class="attvalue">= "White"</span> <span class="attribute">Foreground</span> <span class="attvalue">= "Blue"</span></em> /<span class="bracket">&gt;</span>
</code></pre>
この書き方は <strong id="attribute" class="keyword">Attribute Syntax</strong> と言って、
値を文字列で指定できる（文字列そのもの or 文字列から直接変換可能な型）プロパティの場合はこの構文を使うと便利です。

では、もっと複雑な型を持つプロパティの場合にはどうすればいいかと言うと、
XML 要素の子要素としてプロパティの値を設定する Property Element Syntax という構文も用意されています。
例えば、上の例を <strong id="property" class="keyword">Property Element Syntax</strong> で書き直すと以下のようになります。


<pre class="xsource" title="子要素としてプロパティの値を設定">
<code><span class="bracket">&lt;</span><span class="element">TextBox</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox.Width</span><span class="bracket">&gt;</span>100<span class="bracket">&lt;</span>/<span class="element">TextBox.Width</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox.FontSize</span><span class="bracket">&gt;</span>30<span class="bracket">&lt;</span>/<span class="element">TextBox.FontSize</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox.Background</span><span class="bracket">&gt;</span>White<span class="bracket">&lt;</span>/<span class="element">TextBox.Background</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox.Foreground</span><span class="bracket">&gt;</span>Blue<span class="bracket">&lt;</span>/<span class="element">TextBox.Foreground</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox.Text</span><span class="bracket">&gt;</span>text 1<span class="bracket">&lt;</span>/<span class="element">TextBox.Text</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">TextBox</span><span class="bracket">&gt;</span>
</code></pre>
この例では、Background / Foreground の中身は相変わらず文字列からの自動型変換
（文字列 → SolidColorBrush への変換）
に頼っているわけですが、
これも省略せずに書くなら以下のようになります。


<pre class="xsource" title="子要素としてプロパティの値を設定">
<code><span class="bracket">&lt;</span><span class="element">TextBox</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox.Width</span><span class="bracket">&gt;</span>100<span class="bracket">&lt;</span>/<span class="element">TextBox.Width</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox.FontSize</span><span class="bracket">&gt;</span>30<span class="bracket">&lt;</span>/<span class="element">TextBox.FontSize</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox.Background</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">SolidColorBrush</span> <span class="attribute">Color</span><span class="attvalue">="White"</span>/<span class="bracket">&gt;</span><span class="bracket">&lt;</span>/<span class="element">TextBox.Background</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox.Foreground</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">SolidColorBrush</span> <span class="attribute">Color</span><span class="attvalue">="Blue"</span>/<span class="bracket">&gt;</span><span class="bracket">&lt;</span>/<span class="element">TextBox.Foreground</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox.Text</span><span class="bracket">&gt;</span>text 1<span class="bracket">&lt;</span>/<span class="element">TextBox.Text</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">TextBox</span><span class="bracket">&gt;</span>
</code></pre>
文字列からの自動変換に頼らず、ちゃんとブラシを指定するなら、
グラデーションの掛かった柄（LinearGradientBrush、RadialGradientBrush）や、
画像（ImageBrush）などを背景・前景色に指定する事もできます。

ちなみに、「[Windows Presentation Foundation](wpf_abst.md#wpf)」 でよく使う型には、文字列からの変換関数が標準で用意されているので、
複雑な型でなければたいていは Attribute Syntax が利用できます。


##<a id="sec-generated-title-3"></a> <a id="content"></a>コンテントプロパティ
基本的に、XAML 中のある要素（例えば &lt;Button&gt;）の子は、
その要素に対応するクラス（&lt;Button&gt; の場合、Button クラス）のプロパティになります。


<pre class="xsource" title="子要素は全部 Property Element Syntax">
<code><span class="bracket">&lt;</span><span class="element">Button</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button.Background</span><span class="bracket">&gt;</span>Gray<span class="bracket">&lt;</span>/<span class="element">Button.Background</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button.Foreground</span><span class="bracket">&gt;</span>White<span class="bracket">&lt;</span>/<span class="element">Button.Foreground</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button.Content</span><span class="bracket">&gt;</span>ここを押して<span class="bracket">&lt;</span>/<span class="element">Button.Content</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>
</code></pre>
ただ、
<strong id="content" class="keyword">コンテントプロパティ</strong>（content property）という物に指定されているプロパティに限っては省略が可能になっています。
Button クラスでは、Content がコンテントプロパティに指定されていて、
上の例の &lt;Button.Content&gt; タグは省略可能になり、以下のように書けます。


<pre class="xsource" title="コンテントプロパティはタグを省略可能">
<code><span class="bracket">&lt;</span><span class="element">Button</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button.Background</span><span class="bracket">&gt;</span>Gray<span class="bracket">&lt;</span>/<span class="element">Button.Background</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button.Foreground</span><span class="bracket">&gt;</span>White<span class="bracket">&lt;</span>/<span class="element">Button.Foreground</span><span class="bracket">&gt;</span>
  <em>ここを押して</em>
<span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>
</code></pre>
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


<pre class="xsource" title="コンテントプロパティを分断（エラーになる）">
<code><span class="bracket">&lt;</span><span class="element">Button</span><span class="bracket">&gt;</span>
  <em>他の要素よりも前にテキスト</em>
  <span class="bracket">&lt;</span><span class="element">Button.Background</span><span class="bracket">&gt;</span>Gray<span class="bracket">&lt;</span>/<span class="element">Button.Background</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Button.Foreground</span><span class="bracket">&gt;</span>White<span class="bracket">&lt;</span>/<span class="element">Button.Foreground</span><span class="bracket">&gt;</span>
  <em>かつ、後ろにもテキスト</em>
<span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>
</code></pre>

##<a id="sec-generated-title-4"></a> <a id="collection"></a>プロパティがコレクションの場合
「[基本構造](#structure)」で挙げた例で、
WrapPanel の直下に TextBox タグが書けるのも実はコンテントプロパティによる省略です。


<pre class="xsource" title="XAML でテキストボックス2つを表示">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Foreground</span> <span class="attvalue">= "Blue"</span> <span class="attribute">Text</span> <span class="attvalue">= "text 1"</span> /<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Foreground</span> <span class="attvalue">= "Green"</span> <span class="attribute">Text</span> <span class="attvalue">= "text 2"</span> /<span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
これも、コンテントプロパティを省略せずに書くと以下のようになります。


<pre class="xsource" title="コンテントプロパティを省略せずに表記">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">WrapPanel.Children</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Foreground</span> <span class="attvalue">= "Blue"</span> <span class="attribute">Text</span> <span class="attvalue">= "text 1"</span> /<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Foreground</span> <span class="attvalue">= "Green"</span> <span class="attribute">Text</span> <span class="attvalue">= "text 2"</span> /<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">WrapPanel.Children</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
この例では、実はもう1つ省略しているものがあります。
WrapPanel の「[コンテントプロパティ](#content)」は Children なんですが、
この Children の型は UIElementCollection です。
なので、省略せずに書くなら、
上の例は以下のようになります。


<pre class="xsource" title="コレクションを省略せずに表記">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">WrapPanel.Children</span><span class="bracket">&gt;</span>
    <em><span class="bracket">&lt;</span><span class="element">UIElementCollection</span><span class="bracket">&gt;</span></em>
      <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Foreground</span> <span class="attvalue">= "Blue"</span> <span class="attribute">Text</span> <span class="attvalue">= "text 1"</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">TextBox</span> <span class="attribute">Foreground</span> <span class="attvalue">= "Green"</span> <span class="attribute">Text</span> <span class="attvalue">= "text 2"</span> /<span class="bracket">&gt;</span>
    <em><span class="bracket">&lt;</span><span class="element">/UIElementCollection</span><span class="bracket">&gt;</span></em>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Children</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
要するに、コレクション（IList、IDictionary を実装するクラスか、配列）の場合、タグを1レベル省略することが可能です。


##<a id="sec-generated-title-5"></a> <a id="xmlns"></a>XML 名前空間
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


<pre class="xsource" title="XAML でテキストボックス2つを表示">
<code><span class="bracket">&lt;</span><span class="element">Page</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
</code></pre>
このうち、
http://schemas.microsoft.com/winfx/2006/xaml/presentation の方が WPF で定義されたタグ（クラス群）、
http://schemas.microsoft.com/winfx/2006/xaml の方が XAML の仕様自体に含まれるタグをあらわす XML 名前空間です。

また、XAML では、任意の .NET Framework クラスを XML タグと結びつけることが出来ます。
以下のように、
xmlns:c="clr-namespace:My.Namespace" みたいな書き方をすることで、
My.Namespace 名前空間中のクラス名を XML タグとして利用可能になります。
（コンパイル必須。「[Loose XAML](wpf_xaml.md#loose)」では無理。）


<pre class="xsource" title="System 名前空間内のクラスを利用">
<code><span class="bracket">&lt;</span><span class="element">Page</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">xmlns:sys</span><span class="attvalue">="clr-namespace:System"</span>
  <span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Page.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">sys:DateTime</span> <span class="attribute">x:Key</span><span class="attvalue">="date"</span><span class="bracket">/&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">Page.Resources</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">Page</span><span class="bracket">&gt;</span>
</code></pre>
ちなみに、WPF と同様に、Silverlight も XAML を利用するわけですが、
Silverlight の場合には、
以下のような XML 名前空間を使います。


<pre class="xsource" title="Silverlight の場合">
<code><span class="bracket">&lt;</span><span class="element">Canvas</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/client/2007"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="bracket">&gt;</span>
</code></pre>
http://schemas.microsoft.com/winfx/2006/xaml の方は XAML の仕様自体のものなので、WPF でも Silverlight でも共通です。
