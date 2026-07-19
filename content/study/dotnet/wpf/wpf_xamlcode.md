---
title: "XAML とプログラムコード（WPF）"
source_url: "https://ufcpp.net/study/dotnet/wpf/wpf_xamlcode/"
content_type: "Article"
published_at: "2006-11-19T00:00:00"
updated_at: "2007-06-16T00:00:00"
tags: []
umbraco_id: 1400
parent_id: 1393
sort_order: 6
aliases:
  - "/dotnet/wpf/wpf_xamlcode/"
  - "/dotnet/wpf_xamlcode"
  - "/dotnet/wpf_xamlcode.html"
  - "/study/dotnet/wpf_xamlcode"
  - "/study/dotnet/wpf_xamlcode.html"
---

# XAML とプログラムコード（WPF）

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

XAML で記述した GUI に対して、
C# などのプログラミング言語を用いてイベント処理を記述することができます。

ただし、プログラムコードの埋め込みは、
「[Loose XAML](wpf_xaml.md#loose)」 に対してはできず、必ずコンパイルが必要になります。
（参考：「[XAML のコンパイル](wpf_xaml.md#compile)」。）

↑ JavaScript を使うならコンパイルが不要な、
[Silverlight](http://silverlight.net/) というものもあります。
Silverlight は [Flash](http://ja.wikipedia.org/wiki/Adobe_Flash) の競合となる技術で、
WPF と比べるとかなり機能は制限されていますが、
XAML ベースの GUI アプリケーション開発という点に関しては WPF と同じコンセプトです。


## <a id="sec-generated-title-2"></a> <a id="event"></a>イベント処理

「[プロパティ](../../csharp/oop/oo_property.md#property)」と同様に、
「[イベント](../../csharp/functional/sp_event.md#event)」も
「[Attribute Syntax](wpf_xamlbasic.md#attribute)」 や
「[Property Element Syntax](wpf_xamlbasic.md#property)」 を用いて設定することができます。

例えば、ボタンが押されたときのイベント処理を追加したければ、
以下のように Click イベントを設定します。


<pre class="xsource" title="イベント処理">
<code><span class="bracket">&lt;</span><span class="element">Window</span> <span class="attribute">x:Class</span><span class="attvalue">="XamlWindowsApplication1.Window1"</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Title</span><span class="attvalue">="XAML テストプログラム"</span> <span class="attribute">Height</span><span class="attvalue">="100"</span> <span class="attribute">Width</span><span class="attvalue">="140"</span>
  <span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">x:Code</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>![CDATA[
    private void ButtonClicked(object sender, RoutedEventArgs e)
    {
      MessageBox.Show("ボタンが押されました");
      e.Handled = true;
    }
    ]]<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">x:Code</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Button</span> <em><span class="attribute">Click</span><span class="attvalue">="ButtonClicked"</span></em><span class="bracket">&gt;</span>ここを押して<span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">Window</span><span class="bracket">&gt;</span>
</code></pre>
この例にあるとおり、
プログラムコードは XAML 中の x:Code タグ中に直接埋め込むことも可能です。
（ただし、これは非推奨。次節で説明するコードビハインドを使いましょう。）
x:Code タグも、XML の文法に従う必要があるので、
x:Code タグ中のプログラムコードは CDATA セクションにしてください。
（でないと、&lt; とか &gt; とか &amp; とかの記号が書けない。）

（逆に、XAML を使わなくても、全部 C# などのコードで WPF GUI プログラムを作ることは可能ですが、
ビジュアル/ロジック分離の考え方からするとあまり得策ではありません。）


## <a id="sec-generated-title-3"></a> <a id="codebehind"></a>コードビハインド

イベント処理などのプログラムコードは、先ほどの例のように XAML 中に記述するのではなく、
XAML とは別ファイルにすることが可能です。
このように、XAML で記述した GUI のイベント処理などを別ファイルで与えることを
<strong id="codebehind" class="keyword">コードビハインド</strong>（code-behind）といいます。

例えば、先ほどの例をコードビハインドを使って書き直すと以下のような2つのファイルに分かれます。


<pre class="xsource" title="Window1.xaml">
<code><span class="bracket">&lt;</span><span class="element">Window</span> <span class="attribute">x:Class</span><span class="attvalue">="XamlWindowsApplication1.Window1"</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Title</span><span class="attvalue">="XAML テストプログラム"</span> <span class="attribute">Height</span><span class="attvalue">="100"</span> <span class="attribute">Width</span><span class="attvalue">="140"</span>
  <span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Click</span><span class="attvalue">="ButtonClicked"</span><span class="bracket">&gt;</span>ここを押して<span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">Window</span><span class="bracket">&gt;</span>
</code></pre>
<pre class="source" title="Windows1.xaml.cs" lang="">
<code><span class="reserved">namespace</span> XamlWindowsApplication1
{
  <span class="reserved">public partial class</span> Window1 : System.Windows.Window
  {
    <span class="reserved">public</span> Window1()
    {
      InitializeComponent();
    }

    <span class="reserved">private void</span> ButtonClicked(
      <span class="reserved">object</span> sender, System.Windows.RoutedEventArgs e)
    {
      System.Windows.MessageBox.Show(<span class="literal">"ボタンが押されました"</span>);
      e.Handled = <span class="reserved">true</span>;
    }
  }
}
</code></pre>


XAML では、ルートの Windows 要素の x:Class 属性で、クラスの名前を記述します。
C# コード側では、同名のクラスを partial クラスを使って定義します。
（partial クラスに関しては、「[クラスの分割定義](../../csharp/oop/oo_class.md#partial)」参照。）


## <a id="sec-generated-title-4"></a> <a id="name"></a>GUI 要素の参照

Windows.Forms プログラム（「[GUI アプリケーション](../../csharp/lib/lib_forms.md)」参照）で
<code>this.text1.Text="表示テキスト"</code>
などと記述していたように、
XAML 中に記述した GUI 要素（ボタンなど）をコードビハインド中で読み書きできます。
GUI 要素を参照するための名前を指定するには Name 属性を使います。

例えば、先ほどの例で、メッセージボックスの代わりに、テキストブロックにメッセージを表示する場合、
まず、XAML 側では、以下のように、TextBlock 要素に Name 属性をつけます。


<pre class="xsource" title="Name 属性">
<code><span class="bracket">&lt;</span><span class="element">Window</span> <span class="attribute">x:Class</span><span class="attvalue">="XamlWindowsApplication1.Window1"</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Title</span><span class="attvalue">="XAML テストプログラム"</span> <span class="attribute">Height</span><span class="attvalue">="100"</span> <span class="attribute">Width</span><span class="attvalue">="200"</span>
  <span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">StackPanel</span> <span class="attribute">Orientation</span><span class="attvalue">="Vertical"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Click</span><span class="attvalue">="ButtonClicked"</span><span class="bracket">&gt;</span>ここを押して<span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">TextBlock</span> <em><span class="attribute">Name</span><span class="attvalue">="textBlock"</span></em><span class="bracket">&gt;</span><span class="bracket">&lt;</span>/<span class="element">TextBlock</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">StackPanel</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">Window</span><span class="bracket">&gt;</span>
</code></pre>
これで、このテキストブロックに textBlock という名前が付きました。
C# コード側では、この名前をそのまま変数名として使えます。

<pre class="source" title="GUI 要素の参照" lang="">
<code><span class="reserved">namespace</span> XamlWindowsApplication1
{
  <span class="reserved">public partial class</span> Window1 : System.Windows.Window
  {
    <span class="reserved">public</span> Window1()
    {
      InitializeComponent();
    }

    <span class="reserved">int</span> count = 0;

    <span class="reserved">private void</span> ButtonClicked(
      <span class="reserved">object</span> sender, System.Windows.RoutedEventArgs e)
    {
      ++<span class="reserved">this</span>.count;
      <em><span class="reserved">this</span>.textBlock.Text</em> =
        <span class="reserved">string</span>.Format(<span class="literal">"ボタンが{0}回押されました"</span>, <span class="reserved">this</span>.count);
      e.Handled = <span class="reserved">true</span>;
    }
  }
}
</code></pre>


ちなみに、Button や TextBlock などは、Name プロパティを持っていて、
XAML 中の Name 属性値は、（変数名としてだけでなく）Name プロパティの値にもなります。

一方、Name プロパティを持たない型を XAML 要素として使いたい場合もあるのですが、
その場合、XAML のタグ中には Name 属性は書けません。
このような場合、Name 属性の変わりに x:Name 属性を使います。


## <a id="sec-generated-title-5"></a> <a id="routed"></a>ルーティングイベント

Windows.Forms（「[GUI アプリケーション](../../csharp/lib/lib_forms.md)」参照）では、
ボタンを押されたときとかの処理（イベント処理）は、
C# の「[イベント](../../csharp/functional/sp_event.md#event)」を使って実現していました。

対して、
WPF では、少し複雑なイベント処理方式を採用しています。
「[依存プロパティ](wpf_xamladv.md#dependency)」を用いて、
親要素のプロパティの値を設定したりできたのと同様に、
子要素や親要素で発生したイベントを処理することができます。

例えば、以下のような感じで、
StackPanel の下に連なる Button の Click イベントを、
全部 StackPanel で受けて処理することができます。


<pre class="xsource" title="子要素で生じたのイベントを処理">
<code><span class="bracket">&lt;</span><span class="element">Window</span> <span class="attribute">x:Class</span><span class="attvalue">="XamlWindowsApplication1.MainWindow"</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Title</span><span class="attvalue">="XAML テストプログラム"</span> <span class="attribute">Height</span><span class="attvalue">="100"</span> <span class="attribute">Width</span><span class="attvalue">="200"</span>
  <span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">StackPanel</span> <span class="attribute">Orientation</span><span class="attvalue">="Vertical"</span> <em><span class="attribute">Button.Click</span><span class="attvalue">="ButtonClicked"</span></em><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span><span class="bracket">&gt;</span>ボタン１<span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span><span class="bracket">&gt;</span>ボタン２<span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span><span class="bracket">&gt;</span>ボタン３<span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span><span class="bracket">&gt;</span>ボタン４<span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button</span><span class="bracket">&gt;</span>ボタン５<span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">StackPanel</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">Window</span><span class="bracket">&gt;</span>
</code></pre>
このような仕組みは、
XML ツリーを上にたどってイベントが送られていく（route: 送る）ことから、
<strong id="routed_event" class="keyword">ルーティングイベント</strong>（routed event）と呼ばれています。
（英語だと routed なのに、なぜか和訳はルーティング。）

ちなみに、イベントの発生元（どのボタンが押されたのか）は、
<code>RoutedEventArgs e</code> の <code>e.Source</code> を見れば分かります。
例えば、以下のようにすると、
どのボタンが押されたのかをメッセージボックスで表示するようなプログラムになります。

<pre class="source" title="イベントの発生元" lang="">
<code><span class="reserved">using</span> System.Windows;
<span class="reserved">using</span> System.Windows.Controls;

<span class="reserved">namespace</span> XamlWindowsApplication1
{
  <span class="reserved">public partial class</span> MainWindow : System.Windows.Window
  {
    <span class="reserved">public</span> MainWindow()
    {
      InitializeComponent();
    }

    <span class="reserved">private void</span> ButtonClicked(<span class="reserved">object</span> sender, RoutedEventArgs e)
    {
      Button b = (Button)<em>e.Source</em>;
      MessageBox.Show(<span class="literal">"「"</span> + b.Content.ToString() + <span class="literal">"」が押されました"</span>);
      e.Handled = <span class="reserved">true</span>;
    }
  }
}</code></pre>



## <a id="sec-generated-title-6"></a> <a id="style"></a>スタイル中でのイベントハンドラの設定

（書きかけ）

スタイル中で、
Setter を使ってプロパティの値を設定できたのと同様に、
EventSetter を使ってイベントハンドラを設定できます。


## <a id="sec-generated-title-7"></a> <a id="resource"></a>リソース

（書きかけ）

(TypeName)this.FindResource("Rosource Key");
