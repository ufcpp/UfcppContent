---
title: "【Roslynメタプログラミング】ValueChangedGanerator"
source_url: "https://ufcpp.net/blog/2015/6/valuechangedganerator/"
content_type: "BlogEntry"
published_at: "2015-06-12T13:40:41"
updated_at: "2015-06-12T13:40:41"
tags: []
umbraco_id: 1749
parent_id: 1745
sort_order: 3
aliases: []
---

# 【Roslynメタプログラミング】ValueChangedGanerator

多少やりすぎ感があって大々的に推すかどうかは迷っているものの…

- [ValueChangedGanerator](https://github.com/ufcpp/ValueChangedGanerator)

[.NET Compiler Platform](https://github.com/dotnet/roslyn) (コードネーム Roslyn)を使ったメタプログラミングというか、結構なコード生成ツールを作ってしまっていたり。

[今度やる勉強会](https://csugjp.doorkeeper.jp/events/26140)で.NET Compiler Platformの話をするのと、一度作ってしまえば仕事でも活用できそうだったので。

## 生成ツールの概要

要するに、たびたび出てくる「`INotifyPropertyChanged`の実装めんどくさい問題」に対する、Roslyn使った解決方法の1つ。

`INotifyPropertyChanged`実装はめんどくさいものの、用途が限られてる(ほとんどの場合、クライアント開発者にしか使われない。サーバー側でも`INotifyPropertyChanged`自体は使われることあるけども、そっちは完全に自動生成の中に隠れてたり)のと、実装方法がたまーにカスタマイズしたいことがあって、C#言語構文としてコンパイラーが何か補助するような機能は作りにくいものです。

で、こういうめんどくさいくせに「用途限定」「実装に幅あり」なものに対して有効な解決策がメタプログラミングなわけですが。最近、Roslynを使ったコード生成にだいぶ慣れてきてしまい、作ってみたのがこの[ValueChangedGanerator](https://github.com/ufcpp/ValueChangedGanerator)。

今回僕が作った実装以外にも、つい最近、[ノイエさん](http://neue.cc/)が[NotifyPropertyChangedGenerator](https://github.com/neuecc/NotifyPropertyChangedGenerator/)というやつも作っていたり。というか、これがこっち作るきっかけ。[NotifyPropertyChangedGenerator](https://github.com/neuecc/NotifyPropertyChangedGenerator/)と[こっち](https://github.com/ufcpp/ValueChangedGanerator)の違い(pull-req 送るとかでなく別実装した理由)は:

- プロパティ自身の書き換えでなく、コード生成元にはノータッチで、完全に別の場所にコードを生成
- コード生成先を、同一ファイル内の #region ではなく、別ファイルに分離
- `SetProperty()`まではコード生成せず、こいつは元々ある想定

の3点です。

## 生成方式

- 生成元には、`NotifyRecord`という名前で入れ子になった構造体を定義します。
- この名前の入れ子構造体があると、Quick Action(電球マーク)が出ます
- 「Generate properties」を選択すると、コードが生成されます。

![スクリーンショット](../../../../../assets/media/1019/screenshot.png)

例えば、以下のようなコードが Point.cs と言う名前であったとします。

<pre class="source" title="">
<code><reserved></span><span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Point</span> : <span class="type">BindableBase</span>
{
    <span class="reserved">struct</span> <span class="type">NotifyRecord</span>
    {
        <span class="reserved">public</span> <span class="reserved">int</span> X;
        <span class="reserved">public</span> <span class="reserved">int</span> Y;
        <span class="reserved">public</span> <span class="reserved">int</span> Z =&gt; X * Y;

        <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
        <span class="inactive">///</span><span class="comment"> Name.</span>
        <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
        <span class="reserved">public</span> <span class="reserved">string</span> Name;
    }
}
</code></pre>

このファイル自体も、`Point`クラスに対して`partial`修飾子がつく修正がかかります。

そして、Point.ValueChanged.cs という名前で、以下のようなコードが生成されます。

<pre class="source" title="">
<code><reserved></span><span class="reserved">using</span> System.ComponentModel;

<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">private</span> <span class="type">NotifyRecord</span> _value;

    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span> { <span class="reserved">return</span> _value.X; } <span class="reserved">set</span> { SetProperty(<span class="reserved">ref</span> _value.X, <span class="reserved">value</span>, XProperty); OnPropertyChanged(ZProperty); } }
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type">PropertyChangedEventArgs</span> XProperty = <span class="reserved">new</span> <span class="type">PropertyChangedEventArgs</span>(<span class="reserved">nameof</span>(X));
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span> { <span class="reserved">return</span> _value.Y; } <span class="reserved">set</span> { SetProperty(<span class="reserved">ref</span> _value.Y, <span class="reserved">value</span>, YProperty); OnPropertyChanged(ZProperty); } }
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type">PropertyChangedEventArgs</span> YProperty = <span class="reserved">new</span> <span class="type">PropertyChangedEventArgs</span>(<span class="reserved">nameof</span>(Y));

    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
    <span class="inactive">///</span><span class="comment"> Name.</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
    <span class="reserved">public</span> <span class="reserved">string</span> Name { <span class="reserved">get</span> { <span class="reserved">return</span> _value.Name; } <span class="reserved">set</span> { SetProperty(<span class="reserved">ref</span> _value.Name, <span class="reserved">value</span>, NameProperty); } }
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type">PropertyChangedEventArgs</span> NameProperty = <span class="reserved">new</span> <span class="type">PropertyChangedEventArgs</span>(<span class="reserved">nameof</span>(Name));
    <span class="reserved">public</span> <span class="reserved">int</span> Z =&gt; _value.Z;
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type">PropertyChangedEventArgs</span> ZProperty = <span class="reserved">new</span> <span class="type">PropertyChangedEventArgs</span>(<span class="reserved">nameof</span>(Z));
}
</code></pre>

`SetProperty`と`OnPropertyChanged`の実装は、元のクラス(今回の場合`Point`)が持っている前提でコード生成します。今回は、基底クラスで実装している想定。その基底クラス`BindableBase`は以下のような実装になっています。

<pre class="source" title="">
<code><reserved></span><span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.ComponentModel;
<span class="reserved">using</span> System.Runtime.CompilerServices;

<span class="reserved">class</span> <span class="type">BindableBase</span> : <span class="type">INotifyPropertyChanged</span>
{
    <span class="reserved">#region</span> INotifyPropertyChanged

    <span class="reserved">public</span> <span class="reserved">event</span> <span class="type">PropertyChangedEventHandler</span> PropertyChanged;

    <span class="reserved">protected</span> <span class="reserved">void</span> OnPropertyChanged(<span class="type">PropertyChangedEventArgs</span> args) =&gt; PropertyChanged?.Invoke(<span class="reserved">this</span>, args);

    <span class="reserved">protected</span> <span class="reserved">void</span> OnPropertyChanged([<span class="type">CallerMemberName</span>] <span class="reserved">string</span> propertyName = <span class="reserved">null</span>) =&gt; OnPropertyChanged(<span class="reserved">new</span> <span class="type">PropertyChangedEventArgs</span>(propertyName));

    <span class="reserved">protected</span> <span class="reserved">void</span> SetProperty&lt;<span class="type">T</span>&gt;(<span class="reserved">ref</span> <span class="type">T</span> storage, <span class="type">T</span> value, <span class="type">PropertyChangedEventArgs</span> args)
    {
        <span class="reserved">if</span> (!<span class="type">EqualityComparer</span>&lt;<span class="type">T</span>&gt;.Default.Equals(storage, value))
        {
            storage = value;
            OnPropertyChanged(args);
        }
    }

    <span class="reserved">protected</span> <span class="reserved">void</span> SetProperty&lt;<span class="type">T</span>&gt;(<span class="reserved">ref</span> <span class="type">T</span> storage, <span class="type">T</span> value, [<span class="type">CallerMemberName</span>] <span class="reserved">string</span> propertyName = <span class="reserved">null</span>) =&gt; SetProperty(<span class="reserved">ref</span> storage, value, <span class="reserved">new</span> <span class="type">PropertyChangedEventArgs</span>(propertyName));

    <span class="reserved">#endregion</span>
}
</code></pre>

もちろん、このためにだけに継承を使うのは嫌という話もあります。それはまた[別の方法](https://github.com/ufcpp/MixinGenerator)で解決しようかなぁと(未実装。Test-Firstで先に生成前後のサンプルだけあり)。そっちもやりすぎ感あふれてて悩ましいですが…

## 実装の理由

### 生成元は正規のC#文法

「似て非なる何か」、「亜種」を作らないように、生成元のコードは通常のC#の文法に収まるようにしています。

なので、多少冗長な感じは残ります。今回の実装だと、一段階入れ子にした構造体に元定義を書くので、3行(`struct NotifyRecord`, `{`, `}`)と、1インデント分の無駄はあります。

### 生成は静的に/生成結果がC#コードとして見れる

デバッグ上、生成結果のC#コードが見れるというのは非常に重要。

他の言語で見られるような、動的な処理とか、ビルド後コード挿入でやるようなメタプログラミングは、だいたいデバッグできなくて泣きそうになるので。

.NET Compiler Platformを使ったコード生成でやっているので、この条件は自然と満たします。

### 生成元と生成結果が分かれるように

生成の前後のコードが混ざると、再生成時にどこを書き替えたらいいのか分からなかったり、機械生成の部分に手を入れてしまって再生成で消してしまったりするので。

今回は、元は入れ子の構造体なので、生成元と生成結果がかち合うことはない。
また、`partial`にして別ファイルに分離。

### docコメントも付くように

生成結果が`public`なプロパティなので、ここにdocコメントが付いていてほしい。でないと、「doc XMLを出力する」設定でビルドした時に警告が大量に出てしまうので。

今回は、生成元のフィールドに付けたdocコメントをそのまま移植する実装になっています。

## 利用方法

[github公開してるソースコード](https://github.com/ufcpp/ValueChangedGanerator)の他に、パッケージ化してギャラリーに置いてあります。

- [VSIX(Visual Studio拡張)形式をVisual Studio Galleryに](https://visualstudiogallery.msdn.microsoft.com/bf5dbce2-b73d-4f5f-8e1c-0e1de386b7ce)
- [NuGet形式をNuGet Galleryに](https://www.nuget.org/packages/ValueChangedGanerator/)
