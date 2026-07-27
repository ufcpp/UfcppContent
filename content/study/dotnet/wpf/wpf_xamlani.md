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
  - "/study/dotnet/wpf_xamlani.html"
---

# アニメーション（WPF）

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

「[XAML とプログラムコード（WPF）](wpf_xamlcode.md)」では、
<code>x:Code</code> タグかコードビハインド中にイベントハンドラを記述することで、
イベント処理を行っていました。
これとは別に、イベントトリガやストーリーボードという仕組みを使って、
（コードを含まない）XAML だけでもかなり多彩なイベント処理が可能です。


## <a id="sec-generated-title-2"></a> <a id="review"></a>おさらい

本題のアニメーションの話に入る前に、
「[スタイル](wpf_xamladv.md#style)」とか「[メディア](wpf_uielement.md#Media)」辺りの話を復習。


### <a id="sec-generated-title-3"></a> <a id="Style"></a>Style

複数の要素に一律同じ見た目を適用したい場合、
スタイルというものを使います。


```xml
<WrapPanel
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  Width="200" Height="200" Background="#cccccc">

  <WrapPanel.Resources>
    <Style TargetType="{x:Type Rectangle}">
      <Setter Property="Width" Value="80"/>
      <Setter Property="Height" Value="80"/>
      <Setter Property="Margin" Value="10"/>
      <Setter Property="Fill" Value="#8080ff"/>
    </Style>
  </WrapPanel.Resources>

  <Rectangle />
  <Rectangle />
  <Rectangle />
  <Rectangle />

</WrapPanel>
```

### <a id="sec-generated-title-4"></a> <a id="Brush"></a>Brush

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


```xml
<WrapPanel
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  Width="200" Height="200" Background="#cccccc">

  <WrapPanel.Resources>
    <Style TargetType="{x:Type Rectangle}">
      <Setter Property="Width" Value="90"/>
      <Setter Property="Height" Value="90"/>
      <Setter Property="Margin" Value="5"/>
    </Style>
  </WrapPanel.Resources>

  <!-- 単色塗りつぶし -->
  <Rectangle Fill="MistyRose"/>

  <!-- ↑を Property Element Syntax で書いたもの -->
  <Rectangle>
    <Rectangle.Fill>
      <SolidColorBrush Color="MistyRose"/>
    </Rectangle.Fill>
  </Rectangle>

  <!-- 放射状グラデーション（白黒） -->
  <Rectangle>
    <Rectangle.Fill>
      <RadialGradientBrush>
        <GradientStop Color="#ffffff" Offset="0" />
        <GradientStop Color="#000000" Offset="1" />
      </RadialGradientBrush>
    </Rectangle.Fill>
  </Rectangle>

  <!-- 線形グラデーション（虹色） -->
  <Rectangle>
    <Rectangle.Fill>
      <LinearGradientBrush StartPoint="0,0" EndPoint="1,0">
        <GradientStop Color="#ff8080" Offset="0" />
        <GradientStop Color="#ffc080" Offset="0.125" />
        <GradientStop Color="#ffff80" Offset="0.25" />
        <GradientStop Color="#c0ff80" Offset="0.375" />
        <GradientStop Color="#80ff80" Offset="0.5" />
        <GradientStop Color="#80ffc0" Offset="0.625" />
        <GradientStop Color="#80ffff" Offset="0.75" />
        <GradientStop Color="#80c0ff" Offset="0.875" />
        <GradientStop Color="#8080ff" Offset="1" />
      </LinearGradientBrush>
    </Rectangle.Fill>
  </Rectangle>

</WrapPanel>
```

### <a id="sec-generated-title-5"></a> <a id="Transform"></a>Transform

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


```xml
<WrapPanel
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  Width="200" Height="200" Background="#cccccc">

  <WrapPanel.Resources>
    <Style TargetType="{x:Type Rectangle}">
      <Setter Property="Width" Value="80"/>
      <Setter Property="Height" Value="80"/>
      <Setter Property="Margin" Value="10"/>
      <Setter Property="Fill" Value="#8080ff"/>
    </Style>
  </WrapPanel.Resources>

  <!-- 縦横拡大 -->
  <Rectangle>
    <Rectangle.RenderTransform>
      <ScaleTransform CenterX="50" CenterY="50" ScaleX="0.5" ScaleY="0.5"/>
    </Rectangle.RenderTransform>
  </Rectangle>

  <!-- 回転 -->
  <Rectangle>
    <Rectangle.RenderTransform>
      <RotateTransform CenterX="50" CenterY="50" Angle="10"/>
    </Rectangle.RenderTransform>
  </Rectangle>

  <!-- 拡大＋傾斜＋回転 -->
  <Rectangle>
    <Rectangle.RenderTransform>
      <TransformGroup>
        <ScaleTransform CenterX="0" CenterY="50" ScaleX="1.5" ScaleY="0.5"/>
        <SkewTransform CenterX="100" CenterY="100" AngleX="-20"/>
        <RotateTransform CenterX="50" CenterY="50" Angle="10"/>
      </TransformGroup>
    </Rectangle.RenderTransform>
  </Rectangle>

</WrapPanel>
```

## <a id="sec-generated-title-6"></a> <a id="animation"></a>アニメーション

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



## <a id="sec-generated-title-7"></a> <a id="trigger"></a>イベントトリガ

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


```xml
<WrapPanel
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  Width="200" Height="200" Background="#cccccc">

  <WrapPanel.Resources>
    <Style TargetType="{x:Type Rectangle}">
      <Setter Property="Width" Value="50"/>
      <Setter Property="Height" Value="50"/>
      <Setter Property="Margin" Value="25"/>
      <Setter Property="Fill" Value="#8080ff"/>
    </Style>
  </WrapPanel.Resources>

  <Rectangle>
    <Rectangle.Triggers>
      <EventTrigger  RoutedEvent="Rectangle.Loaded">
        <BeginStoryboard>
          <Storyboard>
            <DoubleAnimation
              Storyboard.TargetProperty="Opacity"
              From="1" To="0.2"
              RepeatBehavior="Forever"
              AutoReverse="true"
              Duration="0:0:1"
              />
          </Storyboard>
        </BeginStoryboard>
      </EventTrigger>
    </Rectangle.Triggers>
  </Rectangle>

</WrapPanel>
```
EventTrigger の子要素 BeginStoryboard に関しては後ほど説明します。
（この例では、表示された瞬間からずっと、
四角形の透明度が薄くなったり濃くなったり点滅し続けます。）

ちなみに、Trigger, EventTrigger の他にも、
Binding で設定した値をトリガにする DataTrigger や、
複数の条件がそろったときに初めてトリガする MultiTrigger などもあります。


## <a id="sec-generated-title-8"></a> <a id="triggerInStyle"></a>スタイル中のイベントトリガ

イベントトリガはスタイル中にも記述できます。

例えば、以下のようにすると、
全ての四角形が同じように点滅し始めます。


```xml
<WrapPanel
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  Width="200" Height="200" Background="#cccccc">

  <WrapPanel.Resources>
    <Style TargetType="{x:Type Rectangle}">
      <Setter Property="Width" Value="50"/>
      <Setter Property="Height" Value="50"/>
      <Setter Property="Margin" Value="25"/>
      <Setter Property="Fill" Value="#8080ff"/>

      <Style.Triggers>
        <EventTrigger  RoutedEvent="Rectangle.Loaded">
          <BeginStoryboard>
            <Storyboard>
              <DoubleAnimation
                Storyboard.TargetProperty="Opacity"
                From="1" To="0.2" Duration="0:0:1"
                RepeatBehavior="Forever" AutoReverse="true" />
            </Storyboard>
          </BeginStoryboard>
        </EventTrigger>
      </Style.Triggers>
    </Style>
  </WrapPanel.Resources>

  <Rectangle/>
  <Rectangle/>
  <Rectangle/>
  <Rectangle/>

</WrapPanel>
```

## <a id="sec-generated-title-9"></a> <a id="action"></a>トリガアクション

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


```xml
<WrapPanel
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  Width="200" Height="200" Background="#cccccc">

  <WrapPanel.Resources>
    <Style TargetType="{x:Type Rectangle}">
      <Setter Property="Width" Value="50"/>
      <Setter Property="Height" Value="50"/>
      <Setter Property="Margin" Value="25"/>
      <Setter Property="Fill" Value="#8080ff"/>
    </Style>
  </WrapPanel.Resources>

  <Rectangle>
    <Rectangle.Triggers>

      <EventTrigger RoutedEvent="Rectangle.Loaded">
        <BeginStoryboard Name="BlinkBegin">
          <Storyboard>
            <DoubleAnimation
              Storyboard.TargetProperty="Opacity"
              From="1" To="0.2" Duration="0:0:1"
              RepeatBehavior="Forever" AutoReverse="true" />
          </Storyboard>
        </BeginStoryboard>
      </EventTrigger>

      <EventTrigger RoutedEvent="Mouse.MouseEnter">
        <PauseStoryboard BeginStoryboardName="BlinkBegin"/>
      </EventTrigger>

      <EventTrigger RoutedEvent="Mouse.MouseLeave">
        <ResumeStoryboard BeginStoryboardName="BlinkBegin"/>
      </EventTrigger>

    </Rectangle.Triggers>
  </Rectangle>

</WrapPanel>
```

## <a id="sec-generated-title-10"></a> <a id="storyboard"></a>ストーリーボード

さて、ようやくアニメーション本体であるストーリーボードの話になります。
ちなみに、<strong id="storyboard" class="keyword">ストーリーボード</strong>（story board）という単語は、
映画やアニメの画コンテ・絵コンテのことです。
要するに、いつ、何を動かすかとか、アニメーションの脚本を描く物。

XAML のストーリーボード（Storyboard）ですが、
DoubleAnimation や ColorAnimation という子要素を複数並べて、
いつ、何の値を、どう変化させるかを指定します。

具体的な説明のために、
先ほどまでにたびたび例示してきた「透明・不透明の点滅」のストーリーボードの部分を抜き出してきてみましょう。


```xml
<Storyboard>
  <DoubleAnimation
    Storyboard.TargetProperty="Opacity"
    From="1" To="0.2" Duration="0:0:1"
    RepeatBehavior="Forever" AutoReverse="true" />
</Storyboard>
```
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


```xml
<WrapPanel
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  Width="200" Height="200" Background="#cccccc">

  <WrapPanel.Resources>
    <Style TargetType="{x:Type Rectangle}">
      <Setter Property="Width" Value="50"/>
      <Setter Property="Height" Value="50"/>
      <Setter Property="Margin" Value="25"/>
      <Setter Property="Fill" Value="#8080ff"/>
    </Style>
  </WrapPanel.Resources>

  <Rectangle Name="rect"/>

  <Button Content="Click Me" Width="80" Height="30">
    <Button.Triggers>
      <EventTrigger RoutedEvent="Button.Click">
        <BeginStoryboard>
          <Storyboard>
            <ColorAnimation
              Storyboard.TargetName="rect"
              Storyboard.TargetProperty="Fill.Color"
              To="#ff8080" Duration="0:0:0"/>
          </Storyboard>
        </BeginStoryboard>
      </EventTrigger>
    </Button.Triggers>
  </Button>
</WrapPanel>
```
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


```xml
<WrapPanel
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  Width="200" Height="200">
  
  <WrapPanel.Resources>
    <Style TargetType="{x:Type Rectangle}">
      <Setter Property="Width" Value="50"/>
      <Setter Property="Height" Value="50"/>
      <Setter Property="Margin" Value="25"/>
      <Setter Property="Fill" Value="#8080ff"/>
      <Setter Property="RenderTransform">
        <Setter.Value>
          <RotateTransform CenterX="25" CenterY="25" Angle="0"/>
        </Setter.Value>
      </Setter>
    </Style>
  </WrapPanel.Resources>

  <Rectangle>
    <Rectangle.Triggers>
      <EventTrigger RoutedEvent="Mouse.MouseEnter">
        <BeginStoryboard>
          <Storyboard>
            <ColorAnimation
              Storyboard.TargetProperty="Fill.Color"
              To="#ff8080" Duration="0:0:0"/>
            <DoubleAnimation
              Storyboard.TargetProperty="RenderTransform.Angle"
              To="0" Duration="0:0:0"/>
          </Storyboard>
        </BeginStoryboard>
      </EventTrigger>
      <EventTrigger RoutedEvent="Mouse.MouseLeave">
        <BeginStoryboard>
          <Storyboard>
            <ColorAnimation
              Storyboard.TargetProperty="Fill.Color"
              To="#8080ff" Duration="0:0:3"/>
            <DoubleAnimation
              Storyboard.TargetProperty="RenderTransform.Angle"
              To="360" Duration="0:0:3"/>
          </Storyboard>
        </BeginStoryboard>
      </EventTrigger>
    </Rectangle.Triggers>
  </Rectangle>

</WrapPanel>
```
サンプル→

[MouseEnter.xaml](../../../../assets/media/ufcpp2000/dotnet/sample/MouseEnter.xaml)
。
Rectangle を4×4で並べたらちょっと面白かった。


##### <a id="sec-generated-title-14"></a>TergetProperty で配列的にアクセス

RenderTransform 属性で、拡大・傾斜・回転などの変形をかけたい場合、
TransformGroup 内に ScaleTransform や RotateTransform などを複数並べることになります。

こういう場合、以下のように、配列的に [0] とか [1] とかを使って TergetProperty を設定することができます。


```xml
<WrapPanel
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  Width="200" Height="200" Background="#cccccc">

  <WrapPanel.Resources>
    <Style TargetType="{x:Type Rectangle}">
      <Setter Property="Width" Value="50"/>
      <Setter Property="Height" Value="50"/>
      <Setter Property="Margin" Value="25"/>
      <Setter Property="Fill" Value="#8080ff"/>
      <Setter Property="RenderTransform">
        <Setter.Value>
          <TransformGroup>
            <RotateTransform CenterX="25" CenterY="25" Angle="0"/>
            <TranslateTransform X="0" Y="0"/>
          </TransformGroup>
        </Setter.Value>
      </Setter>
    </Style>
  </WrapPanel.Resources>

  <Rectangle>
    <Rectangle.Triggers>
      <EventTrigger RoutedEvent="Rectangle.Loaded">
        <BeginStoryboard Name="BlinkBegin">
          <Storyboard>
            <DoubleAnimation
              Storyboard.TargetProperty="RenderTransform.Children[0].Angle"
              From="0" To="360" Duration="0:0:3"
              RepeatBehavior="Forever"/>
            <DoubleAnimation
              Storyboard.TargetProperty="RenderTransform.Children[1].X"
              From="0" To="20" Duration="0:0:0.1212"
              RepeatBehavior="Forever" AutoReverse="true"/>
            <DoubleAnimation
              Storyboard.TargetProperty="RenderTransform.Children[1].Y"
              From="0" To="20" Duration="0:0:0.1413"
              RepeatBehavior="Forever" AutoReverse="true"/>
          </Storyboard>
        </BeginStoryboard>
      </EventTrigger>
    </Rectangle.Triggers>
  </Rectangle>

</WrapPanel>
```
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


## <a id="sec-generated-title-16"></a> <a id="beginanimation"></a>BeginAnimation

XAML だけでアニメーション設定を完結させるには、
これまでに説明したような、イベントトリガ→イベントアクション→ストーリーボードという手順を踏む必要がありますが、
コードビハインド中では、BeginAnimation メソッドを呼び出してアニメーションを開始させることもできます。

（BeginAnimation は Animatable クラスのメソッド。
Contorol や Shape などは Animatable のサブクラス。）

例えば、「[イベントトリガ](#trigger)」節で例に挙げた、
四角形を点滅表示させるものを BeginAnimation を使って書き直すと以下のようになります
（XAML ＋ コードビハインドの C# ファイル）。


```xml
<Window x:Class="WPFApplication1.Window1"
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  xmlns:c="clr-namespace:WPFApplication1"
  Title="Window1" Height="200" Width="200"
  >

  <Rectangle Name="rect1" Width="50" Height="50" Fill="#8080ff"/>
</Window>
```
```xml
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace WPFApplication1
{
  public partial class Window1 : System.Windows.Window
  {
    public Window1()
    {
      InitializeComponent();

      DoubleAnimation ani = new DoubleAnimation(
        1, 0.2, new TimeSpan(0, 0, 1));
      ani.RepeatBehavior = RepeatBehavior.Forever;
      ani.AutoReverse = true;
      this.rect1.BeginAnimation(UIElement.OpacityProperty, ani);
    }
  }
}
```



## <a id="sec-generated-title-17"></a> <a id="CompositionTarget"></a>CompositionTarget.Rendering

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


```xml
<Window x:Class="WPFApplication1.Window1"
  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
  xmlns:c="clr-namespace:WPFApplication1"
  Title="Window1" Height="300" Width="300"
  >

  <Canvas Height="200" Width="200">
    <Canvas.Resources>
      <Style TargetType="Ellipse">
        <Setter Property="Width" Value="10"/>
        <Setter Property="Height" Value="10"/>
        <Setter Property="Fill" Value="#8080ff"/>
      </Style>
    </Canvas.Resources>
    
    <Ellipse Name="obj1" Canvas.Left="30" Canvas.Top="30"/>
    <Ellipse Name="obj2" Canvas.Left="140" Canvas.Top="50"/>
    <Ellipse Name="obj3" Canvas.Left="50" Canvas.Top="140"/>
  </Canvas>
</Window>
```
```xml
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace WPFApplication1
{
  public partial class Window1 : System.Windows.Window
  {
    Point[] x = new Point[3];
    Vector[] v = new Vector[3];
    Shape[] obj = new Shape[3];

    public Window1()
    {
      InitializeComponent();

      this.obj[0] = this.obj1;
      this.obj[1] = this.obj2;
      this.obj[2] = this.obj3;

      for (int i = 0; i < this.obj.Length; ++i)
      {
        x[i] = new Point();
        x[i].X = (double)this.obj[i].GetValue(Canvas.LeftProperty);
        x[i].Y = (double)this.obj[i].GetValue(Canvas.TopProperty);
        v[i] = new Vector();
      }

      CompositionTarget.Rendering +=
        new EventHandler(CompositionTarget_Rendering);
    }

    void CompositionTarget_Rendering(object sender, EventArgs e)
    {
      Vector a01 = x[0] - x[1];
      Vector a12 = x[1] - x[2];
      Vector a20 = x[2] - x[0];

      double abs01 = a01.Length;
      double abs12 = a12.Length;
      double abs20 = a20.Length;

      if (abs01 < 10) abs01 = 10;
      if (abs12 < 10) abs12 = 10;
      if (abs20 < 10) abs20 = 10;

      a01 /= abs01 * abs01;
      a12 /= abs12 * abs12;
      a20 /= abs20 * abs20;

      v[0] += a20 - a01;
      v[1] += a01 - a12;
      v[2] += a12 - a20;

      for (int i = 0; i < this.obj.Length; ++i)
      {
        x[i] += v[i];
        this.obj[i].SetValue(Canvas.LeftProperty, x[i].X);
        this.obj[i].SetValue(Canvas.TopProperty, x[i].Y);
      }
    }
  }
}
```
