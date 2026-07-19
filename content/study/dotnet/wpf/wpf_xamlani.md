---
title: "アニメーション（WPF）"
source_url: "https://ufcpp.net/study/dotnet/wpf/wpf_xamlani/"
content_type: "Article"
published_at: "2007-05-03T00:00:00"
updated_at: "2007-06-16T00:00:00"
tags: []
umbraco_id: 1403
parent_id: 1393
sort_order: 9
aliases:
  - "/dotnet/wpf/wpf_xamlani/"
  - "/dotnet/wpf_xamlani"
  - "/dotnet/wpf_xamlani.html"
  - "/study/dotnet/wpf_xamlani"
  - "/study/dotnet/wpf_xamlani.html"
---

# アニメーション（WPF）

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
「[XAML とプログラムコード（WPF）](wpf_xamlcode.md)」では、
<code>x:Code</code> タグかコードビハインド中にイベントハンドラを記述することで、
イベント処理を行っていました。
これとは別に、イベントトリガやストーリーボードという仕組みを使って、
（コードを含まない）XAML だけでもかなり多彩なイベント処理が可能です。


##<a id="sec-generated-title-2"></a> <a id="review"></a>おさらい
本題のアニメーションの話に入る前に、
「[スタイル](wpf_xamladv.md#style)」とか「[メディア](wpf_uielement.md#Media)」辺りの話を復習。


###<a id="sec-generated-title-3"></a> <a id="Style"></a>Style
複数の要素に一律同じ見た目を適用したい場合、
スタイルというものを使います。


<pre class="xsource" title="スタイルの適用">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span> <span class="attribute">Background</span><span class="attvalue">="#cccccc"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type Rectangle}"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Width"</span> <span class="attribute">Value</span><span class="attvalue">="80"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Height"</span> <span class="attribute">Value</span><span class="attvalue">="80"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Margin"</span> <span class="attribute">Value</span><span class="attvalue">="10"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Fill"</span> <span class="attribute">Value</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Rectangle</span> /<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Rectangle</span> /<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Rectangle</span> /<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Rectangle</span> /<span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>

###<a id="sec-generated-title-4"></a> <a id="Brush"></a>Brush
Shapes なら Fill 属性、
Controls なら Background 属性で、
背景色を指定できるわけですが
（参考:
「[図形](wpf_uielement.md#Shapes)」、
「[コントロール](wpf_uielement.md#Controls)」）、
これらには Brush を指定します。

Brush には、
単色塗りつぶしの SolidColorBrush、
グラデーションをかける
LinearGradientBrush, RadialGradientBrush などがあります。
その他にも、
背景に画像や図形などのパターンを表示する
ImageBrush や DrawingBrush などもあります。

ここでは、主に
SolidColorBrush, LinearGradientBrush, RadialGradientBrush を使って説明するので、
この3つに関して例を挙げておきます。


<pre class="xsource" title="Brush いろいろ">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span> <span class="attribute">Background</span><span class="attvalue">="#cccccc"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type Rectangle}"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Width"</span> <span class="attribute">Value</span><span class="attvalue">="90"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Height"</span> <span class="attribute">Value</span><span class="attvalue">="90"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Margin"</span> <span class="attribute">Value</span><span class="attvalue">="5"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="comment">&lt;!-- 単色塗りつぶし --&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Rectangle</span> <span class="attribute">Fill</span><span class="attvalue">="MistyRose"</span>/<span class="bracket">&gt;</span>

  <span class="comment">&lt;!-- ↑を Property Element Syntax で書いたもの --&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Rectangle</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">SolidColorBrush</span> <span class="attribute">Color</span><span class="attvalue">="MistyRose"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

  <span class="comment">&lt;!-- 放射状グラデーション（白黒） --&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Rectangle</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">RadialGradientBrush</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#ffffff"</span> <span class="attribute">Offset</span><span class="attvalue">="0"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#000000"</span> <span class="attribute">Offset</span><span class="attvalue">="1"</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">RadialGradientBrush</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

  <span class="comment">&lt;!-- 線形グラデーション（虹色） --&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Rectangle</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">LinearGradientBrush</span> <span class="attribute">StartPoint</span><span class="attvalue">="0,0"</span> <span class="attribute">EndPoint</span><span class="attvalue">="1,0"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#ff8080"</span> <span class="attribute">Offset</span><span class="attvalue">="0"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#ffc080"</span> <span class="attribute">Offset</span><span class="attvalue">="0.125"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#ffff80"</span> <span class="attribute">Offset</span><span class="attvalue">="0.25"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#c0ff80"</span> <span class="attribute">Offset</span><span class="attvalue">="0.375"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#80ff80"</span> <span class="attribute">Offset</span><span class="attvalue">="0.5"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#80ffc0"</span> <span class="attribute">Offset</span><span class="attvalue">="0.625"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#80ffff"</span> <span class="attribute">Offset</span><span class="attvalue">="0.75"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#80c0ff"</span> <span class="attribute">Offset</span><span class="attvalue">="0.875"</span> /<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">GradientStop</span> <span class="attribute">Color</span><span class="attvalue">="#8080ff"</span> <span class="attribute">Offset</span><span class="attvalue">="1"</span> /<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">LinearGradientBrush</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Rectangle.Fill</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>

###<a id="sec-generated-title-5"></a> <a id="Transform"></a>Transform
「[メディア](wpf_uielement.md#Media)」で説明したように、
WPF の GUI 要素は、
RenderTransform 属性によって、
拡大・回転などの変形を施すことができます。

x 軸, y 軸方向の拡大を表す ScaleTransform、
軸沿いに斜めに崩すような SkewTransform、
回転を表す RotateTransform、
平行移動を表す TranslateTransform などがあります。
また、MatrixTransform では、行列を使った線形変換もできます
（回転・拡大などと行列の関係は、
「[固有値](../../math/linear/eigen.md)」を参照）。

さらに、TransformGroup を使って複数の変形を一度にかけることもできます。


<pre class="xsource" title="変形いろいろ">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span> <span class="attribute">Background</span><span class="attvalue">="#cccccc"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type Rectangle}"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Width"</span> <span class="attribute">Value</span><span class="attvalue">="80"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Height"</span> <span class="attribute">Value</span><span class="attvalue">="80"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Margin"</span> <span class="attribute">Value</span><span class="attvalue">="10"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Fill"</span> <span class="attribute">Value</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="comment">&lt;!-- 縦横拡大 --&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Rectangle</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.RenderTransform</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">ScaleTransform</span> <span class="attribute">CenterX</span><span class="attvalue">="50"</span> <span class="attribute">CenterY</span><span class="attvalue">="50"</span> <span class="attribute">ScaleX</span><span class="attvalue">="0.5"</span> <span class="attribute">ScaleY</span><span class="attvalue">="0.5"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Rectangle.RenderTransform</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

  <span class="comment">&lt;!-- 回転 --&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Rectangle</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.RenderTransform</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">RotateTransform</span> <span class="attribute">CenterX</span><span class="attvalue">="50"</span> <span class="attribute">CenterY</span><span class="attvalue">="50"</span> <span class="attribute">Angle</span><span class="attvalue">="10"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Rectangle.RenderTransform</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

  <span class="comment">&lt;!-- 拡大＋傾斜＋回転 --&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Rectangle</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.RenderTransform</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">TransformGroup</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">ScaleTransform</span> <span class="attribute">CenterX</span><span class="attvalue">="0"</span> <span class="attribute">CenterY</span><span class="attvalue">="50"</span> <span class="attribute">ScaleX</span><span class="attvalue">="1.5"</span> <span class="attribute">ScaleY</span><span class="attvalue">="0.5"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">SkewTransform</span> <span class="attribute">CenterX</span><span class="attvalue">="100"</span> <span class="attribute">CenterY</span><span class="attvalue">="100"</span> <span class="attribute">AngleX</span><span class="attvalue">="-20"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">RotateTransform</span> <span class="attribute">CenterX</span><span class="attvalue">="50"</span> <span class="attribute">CenterY</span><span class="attvalue">="50"</span> <span class="attribute">Angle</span><span class="attvalue">="10"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">TransformGroup</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Rectangle.RenderTransform</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>

##<a id="sec-generated-title-6"></a> <a id="animation"></a>アニメーション
WPF のアニメーションには、
大まかに分けて以下の三つのものがあります。

* 「イベントトリガ ＋ ストーリーボード」を XAML 中に書く

* コードビハインド中で、BeginAnimation メソッドを呼び出す

* CompositionTarget.Rendering イベントを使う


このうち、ここでは、
XAML だけで書くことのできるイベントリガ ＋ ストーリーボードを中心に説明したいと思います。

XAML のアニメーションはいろいろ複雑ではあるんですが、
概ね、以下の3点を把握すれば大丈夫だと思います。

1. イベントトリガ … 処理を始めるきっかけ。 「イベントが発生した」とか。

2. トリガアクション … 処理の内容。 「アニメーションを開始・一時停止・再開する」など。

3. ストーリーボード … アニメーションの具体的な中身。 コントロールや図形などの変化のさせ方の台本。



##<a id="sec-generated-title-7"></a> <a id="trigger"></a>イベントトリガ
<strong id="trigger" class="keyword">トリガ</strong>というのは、
「プロパティの値が変わった瞬間」とか、
「イベントが発生した瞬間」とかの、
処理を始めるきっかけのことです。

WPF の FrameworkElement（コントロールも図形も、大半、この FrameworkElement のサブクラス）には、
Triggers という名前のプロパティがあります。
この Triggers に対して、
Trigger または EventTrigger を子要素として追加することで、
トリガの設定ができます。

「プロパティの値が変わった瞬間」に処理を行うのが
Trigger なんですが、
こちらはいまいちできることが限られるので、ここでは説明を割愛。

「イベントが発生した瞬間」に処理を行うのが
EventTrigger です。
例えば、「表示された瞬間からアニメーションを開始」（Loaded イベントをトリガにする）ということをしたければ、
以下のようにします。


<pre class="xsource" title="EventTrigger">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span> <span class="attribute">Background</span><span class="attvalue">="#cccccc"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type Rectangle}"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Width"</span> <span class="attribute">Value</span><span class="attvalue">="50"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Height"</span> <span class="attribute">Value</span><span class="attvalue">="50"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Margin"</span> <span class="attribute">Value</span><span class="attvalue">="25"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Fill"</span> <span class="attribute">Value</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Rectangle</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.Triggers</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">EventTrigger</span>  <span class="attribute">RoutedEvent</span><span class="attvalue">="Rectangle.Loaded"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">BeginStoryboard</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">Storyboard</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">DoubleAnimation</span>
              <span class="attribute">Storyboard.TargetProperty</span><span class="attvalue">="Opacity"</span>
              <span class="attribute">From</span><span class="attvalue">="1"</span> <span class="attribute">To</span><span class="attvalue">="0.2"</span>
              <span class="attribute">RepeatBehavior</span><span class="attvalue">="Forever"</span>
              <span class="attribute">AutoReverse</span><span class="attvalue">="true"</span>
              <span class="attribute">Duration</span><span class="attvalue">="0:0:1"</span>
              /<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span>/<span class="element">Storyboard</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">BeginStoryboard</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">EventTrigger</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Rectangle.Triggers</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
EventTrigger の子要素 BeginStoryboard に関しては後ほど説明します。
（この例では、表示された瞬間からずっと、
四角形の透明度が薄くなったり濃くなったり点滅し続けます。）

ちなみに、Trigger, EventTrigger の他にも、
Binding で設定した値をトリガにする DataTrigger や、
複数の条件がそろったときに初めてトリガする MultiTrigger などもあります。


##<a id="sec-generated-title-8"></a> <a id="triggerInStyle"></a>スタイル中のイベントトリガ
イベントトリガはスタイル中にも記述できます。

例えば、以下のようにすると、
全ての四角形が同じように点滅し始めます。


<pre class="xsource" title="スタイル中のイベントトリガ">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span> <span class="attribute">Background</span><span class="attvalue">="#cccccc"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type Rectangle}"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Width"</span> <span class="attribute">Value</span><span class="attvalue">="50"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Height"</span> <span class="attribute">Value</span><span class="attvalue">="50"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Margin"</span> <span class="attribute">Value</span><span class="attvalue">="25"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Fill"</span> <span class="attribute">Value</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>

      <span class="bracket">&lt;</span><span class="element">Style.Triggers</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">EventTrigger</span>  <span class="attribute">RoutedEvent</span><span class="attvalue">="Rectangle.Loaded"</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">BeginStoryboard</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">Storyboard</span><span class="bracket">&gt;</span>
              <span class="bracket">&lt;</span><span class="element">DoubleAnimation</span>
                <span class="attribute">Storyboard.TargetProperty</span><span class="attvalue">="Opacity"</span>
                <span class="attribute">From</span><span class="attvalue">="1"</span> <span class="attribute">To</span><span class="attvalue">="0.2"</span> <span class="attribute">Duration</span><span class="attvalue">="0:0:1"</span>
                <span class="attribute">RepeatBehavior</span><span class="attvalue">="Forever"</span> <span class="attribute">AutoReverse</span><span class="attvalue">="true"</span> /<span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span>/<span class="element">Storyboard</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span>/<span class="element">BeginStoryboard</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">EventTrigger</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">Style.Triggers</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Rectangle</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Rectangle</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Rectangle</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">Rectangle</span>/<span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>

##<a id="sec-generated-title-9"></a> <a id="action"></a>トリガアクション
「処理開始のきっかけ」である EventTrigger の中身には、
「処理の内容」である <strong id="TriggerAction" class="keyword">TriggerAction</strong> というものを指定します。

TriggerAction には、音声データの再生（SoundPlayerAction）などもありますが、
ここでは、
ストーリーボードがらみのものを中心に説明します。

詳しくは次節で説明しますが、
XAML では、ストーリーボードというものを使ってアニメーションを行います。
で、TriggerAction としては、
ストーリーボードの開始（BeginStoryBoard）、
停止（StopStoryBoard）、
一時停止（PauseStoryBoard）、
再開（ResumeStoryBoard）などがあります。

例えば、
以下のようにすると、
表示と同時に点滅を開始して、
マウスが上に乗った瞬間に点滅を一時停止、
マウスが離れた瞬間に点滅を再開できます。


<pre class="xsource" title="ストーリーボードの開始、一時停止、再開">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span> <span class="attribute">Background</span><span class="attvalue">="#cccccc"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type Rectangle}"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Width"</span> <span class="attribute">Value</span><span class="attvalue">="50"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Height"</span> <span class="attribute">Value</span><span class="attvalue">="50"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Margin"</span> <span class="attribute">Value</span><span class="attvalue">="25"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Fill"</span> <span class="attribute">Value</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Rectangle</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.Triggers</span><span class="bracket">&gt;</span>

      <span class="bracket">&lt;</span><span class="element">EventTrigger</span> <span class="attribute">RoutedEvent</span><span class="attvalue">="Rectangle.Loaded"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">BeginStoryboard</span> <span class="attribute">Name</span><span class="attvalue">="BlinkBegin"</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">Storyboard</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">DoubleAnimation</span>
              <span class="attribute">Storyboard.TargetProperty</span><span class="attvalue">="Opacity"</span>
              <span class="attribute">From</span><span class="attvalue">="1"</span> <span class="attribute">To</span><span class="attvalue">="0.2"</span> <span class="attribute">Duration</span><span class="attvalue">="0:0:1"</span>
              <span class="attribute">RepeatBehavior</span><span class="attvalue">="Forever"</span> <span class="attribute">AutoReverse</span><span class="attvalue">="true"</span> /<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span>/<span class="element">Storyboard</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">BeginStoryboard</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">EventTrigger</span><span class="bracket">&gt;</span>

      <span class="bracket">&lt;</span><span class="element">EventTrigger</span> <span class="attribute">RoutedEvent</span><span class="attvalue">="Mouse.MouseEnter"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">PauseStoryboard</span> <span class="attribute">BeginStoryboardName</span><span class="attvalue">="BlinkBegin"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">EventTrigger</span><span class="bracket">&gt;</span>

      <span class="bracket">&lt;</span><span class="element">EventTrigger</span> <span class="attribute">RoutedEvent</span><span class="attvalue">="Mouse.MouseLeave"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">ResumeStoryboard</span> <span class="attribute">BeginStoryboardName</span><span class="attvalue">="BlinkBegin"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">EventTrigger</span><span class="bracket">&gt;</span>

    <span class="bracket">&lt;</span>/<span class="element">Rectangle.Triggers</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>

##<a id="sec-generated-title-10"></a> <a id="storyboard"></a>ストーリーボード
さて、ようやくアニメーション本体であるストーリーボードの話になります。
ちなみに、<strong id="storyboard" class="keyword">ストーリーボード</strong>（story board）という単語は、
映画やアニメの画コンテ・絵コンテのことです。
要するに、いつ、何を動かすかとか、アニメーションの脚本を描く物。

XAML のストーリーボード（Storyboard）ですが、
DoubleAnimation や ColorAnimation という子要素を複数並べて、
いつ、何の値を、どう変化させるかを指定します。

具体的な説明のために、
先ほどまでにたびたび例示してきた「透明・不透明の点滅」のストーリーボードの部分を抜き出してきてみましょう。


<pre class="xsource" title="Storyboard">
<code><span class="bracket">&lt;</span><span class="element">Storyboard</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">DoubleAnimation</span>
    <span class="attribute">Storyboard.TargetProperty</span><span class="attvalue">="Opacity"</span>
    <span class="attribute">From</span><span class="attvalue">="1"</span> <span class="attribute">To</span><span class="attvalue">="0.2"</span> <span class="attribute">Duration</span><span class="attvalue">="0:0:1"</span>
    <span class="attribute">RepeatBehavior</span><span class="attvalue">="Forever"</span> <span class="attribute">AutoReverse</span><span class="attvalue">="true"</span> /<span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">Storyboard</span><span class="bracket">&gt;</span>
</code></pre>
DoubleAnimation というのは、名前どおり、
double 型の値を変化させるものです。
DoubleAnimation の他にも、「型名 ＋ Animation」という名前のクラスがいくつかあって、
いずれもその型の値を変化させるためのものです。
（例えば、ColorAnimation、CharAnimation、PointAnimation などがあります。）

何の値を変えるかは Storyboard.TargetProperty 属性で指定します。
この例の場合、Rectangle の中にこのストーリーボードが書かれているので、
Rectangle の Opacity（透明度）プロパティの値が変化します。

値を何から何に変化させるかは、From, To 属性で指定します。
「どこからどこまで」ではなくて、
「変化量」を指定したい場合には By 属性を使います。

From から To の値まで、どのくらいの時間かけて変化させるかは Duration で指定します。
Duration の中身には、この例の場合「0:0:1」と書かれていますが、
これは「0時間0分1秒」という意味です。
要するに、「時:分:秒」という形式で指定します。

その他、この例では、To の値に達した後、逆に From の値に戻るのかどうかを表す AutoReverse="true" と、
その後さらに、永久ループするかどうかを表す RepeatBehavior="Forever" が指定されています。

また、この例の場合、
イベントがトリガされた瞬間からアニメーションを開始しているので省略されていますが、
開始時間を遅らせたい場合、
BeginTime 属性を指定します。


##### <a id="sec-generated-title-11"></a>TargetName
上記の場合、
Rectangle 内でトリガしたイベント内で、
Rectangle のプロパティの値を変更していますが、
「ボタンを押したときに Rectangle の背景色を変える」というように、
トリガ主とアニメーションのターゲットを別にすることもできます。
これには、以下のように、Storyboard.TargetName 属性を使います。


<pre class="xsource" title="Storyboard.TargetName">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span> <span class="attribute">Background</span><span class="attvalue">="#cccccc"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type Rectangle}"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Width"</span> <span class="attribute">Value</span><span class="attvalue">="50"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Height"</span> <span class="attribute">Value</span><span class="attvalue">="50"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Margin"</span> <span class="attribute">Value</span><span class="attvalue">="25"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Fill"</span> <span class="attribute">Value</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Rectangle</span> <span class="attribute">Name</span><span class="attvalue">="rect"</span>/<span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Button</span> <span class="attribute">Content</span><span class="attvalue">="Click Me"</span> <span class="attribute">Width</span><span class="attvalue">="80"</span> <span class="attribute">Height</span><span class="attvalue">="30"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Button.Triggers</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">EventTrigger</span> <span class="attribute">RoutedEvent</span><span class="attvalue">="Button.Click"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">BeginStoryboard</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">Storyboard</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">ColorAnimation</span>
              <em><span class="attribute">Storyboard.TargetName</span><span class="attvalue">="rect"</span></em>
              <span class="attribute">Storyboard.TargetProperty</span><span class="attvalue">="Fill.Color"</span>
              <span class="attribute">To</span><span class="attvalue">="#ff8080"</span> <span class="attribute">Duration</span><span class="attvalue">="0:0:0"</span>/<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span>/<span class="element">Storyboard</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">BeginStoryboard</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">EventTrigger</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Button.Triggers</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Button</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
サンプル→

[ButtonClick.xaml](../../../../assets/media/ufcpp2000/dotnet/sample/ButtonClick.xaml)
。
4色版。


##### <a id="sec-generated-title-12"></a>TergetProperty を階層的に指定
この例では、
ボタンクリック後の Rectangle の色を、
Storyboard.TargetProperty="Fill.Color"
で指定しています。
これは、省略せずにきちんと書くなら、
Storyboard.TargetProperty="(Shape.Fill).(SolidColorBrush.Color)"
となります。
このように、TargetProperty には、階層的なプロパティの指定の仕方ができます。

ところで、
Shape クラスの Fill プロパティは、
実際には Brush クラス（「[抽象クラス](../../csharp/oop/oo_abstract.md#abclass)」）です。
Shape.Fill に 「[Attribute Syntax](wpf_xamlbasic.md#attribute)」 で色を設定すると、
自動的に SolidColorBrush に変換されるので、
この例の場合はこれでうまくいきます。
対して、Fill に LinearGradientBrush などを設定しているとうまく動作しません。
（エラーになったりはしないけども、何も起きない。）


##### <a id="sec-generated-title-13"></a>複数のアニメーションを設定
ストーリーボード内には、複数のアニメーションを同時に指定できます。

以下の例では、色の変化と回転を同時に行っています。
この例では、Rectangle の上にマウスを乗せると、
Rectangle の色が変わって回転し始めます（3秒間）。


<pre class="xsource" title="色の変化と回転を同時にアニメーション">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span><span class="bracket">&gt;</span>
  
  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type Rectangle}"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Width"</span> <span class="attribute">Value</span><span class="attvalue">="50"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Height"</span> <span class="attribute">Value</span><span class="attvalue">="50"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Margin"</span> <span class="attribute">Value</span><span class="attvalue">="25"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Fill"</span> <span class="attribute">Value</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="RenderTransform"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Setter.Value</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">RotateTransform</span> <span class="attribute">CenterX</span><span class="attvalue">="25"</span> <span class="attribute">CenterY</span><span class="attvalue">="25"</span> <span class="attribute">Angle</span><span class="attvalue">="0"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">Setter.Value</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">Setter</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Rectangle</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.Triggers</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">EventTrigger</span> <span class="attribute">RoutedEvent</span><span class="attvalue">="Mouse.MouseEnter"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">BeginStoryboard</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">Storyboard</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">ColorAnimation</span>
              <span class="attribute">Storyboard.TargetProperty</span><span class="attvalue">="Fill.Color"</span>
              <span class="attribute">To</span><span class="attvalue">="#ff8080"</span> <span class="attribute">Duration</span><span class="attvalue">="0:0:0"</span>/<span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">DoubleAnimation</span>
              <span class="attribute">Storyboard.TargetProperty</span><span class="attvalue">="RenderTransform.Angle"</span>
              <span class="attribute">To</span><span class="attvalue">="0"</span> <span class="attribute">Duration</span><span class="attvalue">="0:0:0"</span>/<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span>/<span class="element">Storyboard</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">BeginStoryboard</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">EventTrigger</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">EventTrigger</span> <span class="attribute">RoutedEvent</span><span class="attvalue">="Mouse.MouseLeave"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">BeginStoryboard</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">Storyboard</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">ColorAnimation</span>
              <span class="attribute">Storyboard.TargetProperty</span><span class="attvalue">="Fill.Color"</span>
              <span class="attribute">To</span><span class="attvalue">="#8080ff"</span> <span class="attribute">Duration</span><span class="attvalue">="0:0:3"</span>/<span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">DoubleAnimation</span>
              <span class="attribute">Storyboard.TargetProperty</span><span class="attvalue">="RenderTransform.Angle"</span>
              <span class="attribute">To</span><span class="attvalue">="360"</span> <span class="attribute">Duration</span><span class="attvalue">="0:0:3"</span>/<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span>/<span class="element">Storyboard</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">BeginStoryboard</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">EventTrigger</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Rectangle.Triggers</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
サンプル→

[MouseEnter.xaml](../../../../assets/media/ufcpp2000/dotnet/sample/MouseEnter.xaml)
。
Rectangle を4×4で並べたらちょっと面白かった。


##### <a id="sec-generated-title-14"></a>TergetProperty で配列的にアクセス
RenderTransform 属性で、拡大・傾斜・回転などの変形をかけたい場合、
TransformGroup 内に ScaleTransform や RotateTransform などを複数並べることになります。

こういう場合、以下のように、配列的に [0] とか [1] とかを使って TergetProperty を設定することができます。


<pre class="xsource" title="TergetProperty に配列的にアクセス">
<code><span class="bracket">&lt;</span><span class="element">WrapPanel</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Width</span><span class="attvalue">="200"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span> <span class="attribute">Background</span><span class="attvalue">="#cccccc"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="{x:Type Rectangle}"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Width"</span> <span class="attribute">Value</span><span class="attvalue">="50"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Height"</span> <span class="attribute">Value</span><span class="attvalue">="50"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Margin"</span> <span class="attribute">Value</span><span class="attvalue">="25"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Fill"</span> <span class="attribute">Value</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="RenderTransform"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Setter.Value</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">TransformGroup</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">RotateTransform</span> <span class="attribute">CenterX</span><span class="attvalue">="25"</span> <span class="attribute">CenterY</span><span class="attvalue">="25"</span> <span class="attribute">Angle</span><span class="attvalue">="0"</span>/<span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">TranslateTransform</span> <span class="attribute">X</span><span class="attvalue">="0"</span> <span class="attribute">Y</span><span class="attvalue">="0"</span>/<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span>/<span class="element">TransformGroup</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">Setter.Value</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">Setter</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">WrapPanel.Resources</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Rectangle</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Rectangle.Triggers</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">EventTrigger</span> <span class="attribute">RoutedEvent</span><span class="attvalue">="Rectangle.Loaded"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">BeginStoryboard</span> <span class="attribute">Name</span><span class="attvalue">="BlinkBegin"</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">Storyboard</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">DoubleAnimation</span>
              <span class="attribute">Storyboard.TargetProperty</span><span class="attvalue">="RenderTransform.Children[0].Angle"</span>
              <span class="attribute">From</span><span class="attvalue">="0"</span> <span class="attribute">To</span><span class="attvalue">="360"</span> <span class="attribute">Duration</span><span class="attvalue">="0:0:3"</span>
              <span class="attribute">RepeatBehavior</span><span class="attvalue">="Forever"</span>/<span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">DoubleAnimation</span>
              <span class="attribute">Storyboard.TargetProperty</span><span class="attvalue">="RenderTransform.Children[1].X"</span>
              <span class="attribute">From</span><span class="attvalue">="0"</span> <span class="attribute">To</span><span class="attvalue">="20"</span> <span class="attribute">Duration</span><span class="attvalue">="0:0:0.1212"</span>
              <span class="attribute">RepeatBehavior</span><span class="attvalue">="Forever"</span> <span class="attribute">AutoReverse</span><span class="attvalue">="true"</span>/<span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">DoubleAnimation</span>
              <span class="attribute">Storyboard.TargetProperty</span><span class="attvalue">="RenderTransform.Children[1].Y"</span>
              <span class="attribute">From</span><span class="attvalue">="0"</span> <span class="attribute">To</span><span class="attvalue">="20"</span> <span class="attribute">Duration</span><span class="attvalue">="0:0:0.1413"</span>
              <span class="attribute">RepeatBehavior</span><span class="attvalue">="Forever"</span> <span class="attribute">AutoReverse</span><span class="attvalue">="true"</span>/<span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span>/<span class="element">Storyboard</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span>/<span class="element">BeginStoryboard</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">EventTrigger</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Rectangle.Triggers</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Rectangle</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span>/<span class="element">WrapPanel</span><span class="bracket">&gt;</span>
</code></pre>
サンプル→

[Gradation.xaml](../../../../assets/media/ufcpp2000/dotnet/sample/Gradation.xaml)
。
LinearGradientBrush の GradientStops なんかも配列的にアクセス。
グラデーションの色をアニメーションして、回転・拡大・平行移動を同時にかけたら、
なかなか気持ち悪いのができた。


##### <a id="sec-generated-title-15"></a>その他のアニメーション方式
DoubleAnimation などを使うと、
From から To の値に線形に値が変化します。
これに対して、もう少し凝った値の変化のさせ方もできます。

例えば、
DoubleAnimationUsingKeyFrame
を使えば、
「時刻 xx に値 XX に、
時刻 yy に値 YY に、・・・」
というように、「いつ何の値にするか」を複数並べてアニメーションを作ることができます。

また、
DoubleAnimationUsingPath
を使えば、
パス（複数の点をベジエ補間やスプライン補間で滑らかにつないだもの）に沿って値を変化させることができます。


##<a id="sec-generated-title-16"></a> <a id="beginanimation"></a>BeginAnimation
XAML だけでアニメーション設定を完結させるには、
これまでに説明したような、イベントトリガ→イベントアクション→ストーリーボードという手順を踏む必要がありますが、
コードビハインド中では、BeginAnimation メソッドを呼び出してアニメーションを開始させることもできます。

（BeginAnimation は Animatable クラスのメソッド。
Contorol や Shape などは Animatable のサブクラス。）

例えば、「[イベントトリガ](#trigger)」節で例に挙げた、
四角形を点滅表示させるものを BeginAnimation を使って書き直すと以下のようになります
（XAML ＋ コードビハインドの C# ファイル）。


<pre class="xsource" title="Windows1.xaml">
<code><span class="bracket">&lt;</span><span class="element">Window</span> <span class="attribute">x:Class</span><span class="attvalue">="WPFApplication1.Window1"</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">xmlns:c</span><span class="attvalue">="clr-namespace:WPFApplication1"</span>
  <span class="attribute">Title</span><span class="attvalue">="Window1"</span> <span class="attribute">Height</span><span class="attvalue">="200"</span> <span class="attribute">Width</span><span class="attvalue">="200"</span>
  <span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Rectangle</span> <span class="attribute">Name</span><span class="attvalue">="rect1"</span> <span class="attribute">Width</span><span class="attvalue">="50"</span> <span class="attribute">Height</span><span class="attvalue">="50"</span> <span class="attribute">Fill</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">Window</span><span class="bracket">&gt;</span>
</code></pre>
<pre class="source" title="Windows1.xaml.cs" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Windows;
<span class="reserved">using</span> System.Windows.Media.Animation;

<span class="reserved">namespace</span> WPFApplication1
{
  <span class="reserved">public partial class</span> Window1 : System.Windows.Window
  {
    <span class="reserved">public</span> Window1()
    {
      InitializeComponent();

<em>      DoubleAnimation ani = <span class="reserved">new</span> DoubleAnimation(
        1, 0.2, <span class="reserved">new</span> TimeSpan(0, 0, 1));
      ani.RepeatBehavior = RepeatBehavior.Forever;
      ani.AutoReverse = <span class="reserved">true</span>;
      <span class="reserved">this</span>.rect1.BeginAnimation(UIElement.OpacityProperty, ani);</em>
    }
  }
}
</code></pre>



##<a id="sec-generated-title-17"></a> <a id="CompositionTarget"></a>CompositionTarget.Rendering
ストーリーボードや BeginAnimation によるアニメーションは、
「タイムラインベース」です。

コンピュータ上のアニメーションというのは、連続的に動いているように見えて、
実はパラパラ漫画のような離散的なものです。
人間の目をごまかせるくらい高速に絵を切り替えることで、
動いている用に見えています。

で、タイムラインベースのアニメーションでは、
「時刻 <span class="math">
        t<sub><span class="normal">1</span></sub>
      </span> に位置 <span class="math">
        x<sub><span class="normal">1</span></sub>
      </span>、
時刻 <span class="math">
        t<sub><span class="normal">2</span></sub>
      </span> に位置 <span class="math">
        x<sub><span class="normal">2</span></sub>
      </span> にある」
というような情報を基にして、
「じゃあ、時刻 <span class="math">t</span>
（<span class="math">
        t<sub><span class="normal">1</span></sub><span class="normal">&lt;</span> t <span class="normal">&lt;</span> t<sub><span class="normal">2</span></sub>
      </span>）では位置 <span class="math">x</span> にいるはずだ」
という値を計算して、その位置に物体を表示させます。

このような方式とは別に、
物理シミュレーションなんかでは、
「1フレームごとに逐次的に値を更新」とかいう方式で値を計算したい場合があります。
（加速度 <span class="math">a</span> を与えて、
毎時刻 <span class="math">
        x <span class="normal">=</span> x <span class="normal">+</span> v
      </span>, <span class="math">
        v <span class="normal">=</span> v <span class="normal">+</span> a
      </span> という更新式にしたがって値を更新したり。）

こういう「1フレームごとに処理」という処理を実現するために、
「画面がレンダリングされるタイミングを拾えるイベント」が用意されています。
それが System.Windows.Media.CompositionTarget クラスの Rendering イベント（静的イベント）です。

例として、距離に反比例する引力が働く3つの物体の運動のシミュレーションを示します。


<pre class="xsource" title="Windows1.xaml">
<code><span class="bracket">&lt;</span><span class="element">Window</span> <span class="attribute">x:Class</span><span class="attvalue">="WPFApplication1.Window1"</span>
  <span class="attribute">xmlns</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">="http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">xmlns:c</span><span class="attvalue">="clr-namespace:WPFApplication1"</span>
  <span class="attribute">Title</span><span class="attvalue">="Window1"</span> <span class="attribute">Height</span><span class="attvalue">="300"</span> <span class="attribute">Width</span><span class="attvalue">="300"</span>
  <span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Canvas</span> <span class="attribute">Height</span><span class="attvalue">="200"</span> <span class="attribute">Width</span><span class="attvalue">="200"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Canvas.Resources</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">Style</span> <span class="attribute">TargetType</span><span class="attvalue">="Ellipse"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Width"</span> <span class="attribute">Value</span><span class="attvalue">="10"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Height"</span> <span class="attribute">Value</span><span class="attvalue">="10"</span>/<span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">Setter</span> <span class="attribute">Property</span><span class="attvalue">="Fill"</span> <span class="attribute">Value</span><span class="attvalue">="#8080ff"</span>/<span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span>/<span class="element">Style</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span>/<span class="element">Canvas.Resources</span><span class="bracket">&gt;</span>
    
    <span class="bracket">&lt;</span><span class="element">Ellipse</span> <span class="attribute">Name</span><span class="attvalue">="obj1"</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="30"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="30"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Ellipse</span> <span class="attribute">Name</span><span class="attvalue">="obj2"</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="140"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="50"</span>/<span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">Ellipse</span> <span class="attribute">Name</span><span class="attvalue">="obj3"</span> <span class="attribute">Canvas.Left</span><span class="attvalue">="50"</span> <span class="attribute">Canvas.Top</span><span class="attvalue">="140"</span>/<span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span>/<span class="element">Canvas</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">Window</span><span class="bracket">&gt;</span>
</code></pre>
<pre class="source" title="Windows1.xaml.cs" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Windows;
<span class="reserved">using</span> System.Windows.Controls;
<span class="reserved">using</span> System.Windows.Shapes;
<span class="reserved">using</span> System.Windows.Media;

<span class="reserved">namespace</span> WPFApplication1
{
  <span class="reserved">public partial class</span> Window1 : System.Windows.Window
  {
    Point[] x = <span class="reserved">new</span> Point[3];
    Vector[] v = <span class="reserved">new</span> Vector[3];
    Shape[] obj = <span class="reserved">new</span> Shape[3];

    <span class="reserved">public</span> Window1()
    {
      InitializeComponent();

      <span class="reserved">this</span>.obj[0] = <span class="reserved">this</span>.obj1;
      <span class="reserved">this</span>.obj[1] = <span class="reserved">this</span>.obj2;
      <span class="reserved">this</span>.obj[2] = <span class="reserved">this</span>.obj3;

      <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; <span class="reserved">this</span>.obj.Length; ++i)
      {
        x[i] = <span class="reserved">new</span> Point();
        x[i].X = (<span class="reserved">double</span>)<span class="reserved">this</span>.obj[i].GetValue(Canvas.LeftProperty);
        x[i].Y = (<span class="reserved">double</span>)<span class="reserved">this</span>.obj[i].GetValue(Canvas.TopProperty);
        v[i] = <span class="reserved">new</span> Vector();
      }

      <em>CompositionTarget.Rendering +=
        <span class="reserved">new</span> EventHandler(CompositionTarget_Rendering);</em>
    }

    <span class="reserved">void</span> CompositionTarget_Rendering(<span class="reserved">object</span> sender, EventArgs e)
    {
      Vector a01 = x[0] - x[1];
      Vector a12 = x[1] - x[2];
      Vector a20 = x[2] - x[0];

      <span class="reserved">double</span> abs01 = a01.Length;
      <span class="reserved">double</span> abs12 = a12.Length;
      <span class="reserved">double</span> abs20 = a20.Length;

      <span class="reserved">if</span> (abs01 &lt; 10) abs01 = 10;
      <span class="reserved">if</span> (abs12 &lt; 10) abs12 = 10;
      <span class="reserved">if</span> (abs20 &lt; 10) abs20 = 10;

      a01 /= abs01 * abs01;
      a12 /= abs12 * abs12;
      a20 /= abs20 * abs20;

      v[0] += a20 - a01;
      v[1] += a01 - a12;
      v[2] += a12 - a20;

      <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; <span class="reserved">this</span>.obj.Length; ++i)
      {
        x[i] += v[i];
        <span class="reserved">this</span>.obj[i].SetValue(Canvas.LeftProperty, x[i].X);
        <span class="reserved">this</span>.obj[i].SetValue(Canvas.TopProperty, x[i].Y);
      }
    }
  }
}
</code></pre>
