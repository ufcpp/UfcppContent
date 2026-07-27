---
title: "今更ながら LaTeX"
source_url: "https://ufcpp.net/study/misc/list/latex/"
content_type: "Article"
published_at: "2015-05-06T14:19:03"
updated_at: "2015-05-06T14:19:03"
tags: []
umbraco_id: 1535
parent_id: 1534
sort_order: 0
aliases:
  - "/study/misc/latex.html"
---

# 今更ながら LaTeX

## <a id="sec-generated-title-1"></a> <a id="d22e4"></a>概要

今の世の中、文書ファイルの標準的なフォーマットと言えばMicrosoftのWordドキュメントかAdobeのPDFでしょうか。
ウェブ上に公開するにしてもXMLかHTMLが一般的です。
そんな中LaTeXが根強く生き残ってるのは、アンチM$な人たちがWordを使わないのと、ただであることと、あと数式を書くならやっぱりLaTeXが一番楽だからでしょうか。
ちなみに、僕がLaTeX使ってる理由は3つ目の理由、つまり数式を使いたいからです。
 
とはいえ、LaTeXっていうとリファレンス本片手に持ちながらでないととてもじゃないけれども使えません。
たいてい、家でLaTeXを使うときには本を片手にLaTeXを使っているのですが、
家でも研究室でもLaTeXを使いたいけれでも本は1冊しかなくて困っています。
 
そういうわけでネット上でLaTeX小ネタ集を作って家からも研究室からも見れるようにしようというのがこのページの目的です。
 
とりあえず、LaTeXの解説ページなら検索エンジンで探せばいくらでも出てくるので、
そういう入門的な事をいまさら新たにかくのもどうかと思うのでパス。
このページでは小ネタ集的なノリで行こうかと思っています。


## <a id="sec-generated-title-2"></a> <a id="vector"></a>太字イタリックのベクトル

LaTeXで<code>\vector</code>というと、<ruby><rb>AB</rb><rt>→</rt><rp>(IE5でしか見れないけど許してね)</rp></ruby>というような感じの文字の上に矢印が乗っかったものになります。
太字イタリック体のベクトルを書くには自分でマクロ定義してやる必要があります。

```csharp
\def\Vec#1{\mbox{\boldmath $#1$}}
```



## <a id="sec-generated-title-3"></a> <a id="diff"></a>微分記号

微分記号のdなんですけども、ブロック体で書いたほうがいいってことになってます。
要するに
<table class="layout" summary="レイアウト用テーブル">
<tr><td>悪い例</td><td><i>dx</i></td></tr><tr><td>良い例</td><td>d<i>x</i></td></tr></table>


LaTeXだとめんどくさいからついつい、
<code>$\frac{dx}{dt}</code>
なんて書いちゃうんですが、こうすると微分のdがイタリック体になってしまいます。
少々めんどくさいのですが、

```csharp
\def\diff{\mathrm d}
```


と言うのを定義して
<code>$\frac{\diff x}{\diff t}</code>
としましょう。
ちなみに、数学定数(指数関数の底eとか虚数単位i)もブロック体で書いたほうがいいことになってます。
ついでに、dx/dtをいちいち<code>\frac{\diff x}{\diff t}</code>と書くのが面倒なときは

```csharp
\def\ddt#1{\frac{\diff #1}{\diff t}}
```


というのを定義しておいて<code>\ddt{x}</code>とすれば楽でいいかも。


## <a id="sec-generated-title-4"></a> <a id="fourier"></a>ラプラス変換・フーリエ変換

ラプラス変換とかフーリエ変換の記号を出したいとき、要するに筆記体をかければ言い訳ですが、そのためには<code>\cal</code>という命令を使います。

```csharp
\def\laplace#1#2{{\cal L}\left[#1\right]\left(#2\right)}
\def\fourier#1#2{{\cal F}\left[#1\right]\left(#2\right)}
```



## <a id="sec-generated-title-5"></a> <a id="dint"></a>角度、温度

角度(45°とか)や温度(21℃とか)を表示したいとき、まあ、全角文字の°とか℃を使っちゃえばすむんですが、一応LaTeXで書こうってことで

```csharp
\def\degree{\kern-.2em\r{}\kern-.3em}
\def\degC{\kern-.2em\r{}\kern-.3em C}
```



## <a id="sec-generated-title-6"></a> <a id="dint"></a>重積分

重積分を書くとき、普通に<code>\int\int_Df(x,y)\diff x\diff y</code>と書くと積分記号の間のスペースが空きすぎてちょっと不恰好です。そこで

```csharp
\def\dint{\int\!\!\!\int}
```


というなマクロを定義して<code>\dint_Df(x,y)\diff x\diff y</code>とします。


## <a id="sec-generated-title-7"></a> <a id="praphicx"></a>画像を貼り付けたい

画像を貼り付けたいってのはよくある要望なんですが、
TeXは文章を書くのが専門なんでTeX自身は画像ファイルを取り込む機能を持っていません
(それに画像ってのはWindowsではBMPが標準形式だし、Macだとpictやtiff、UNIXではPBMが標準的というように機種ごとに違うのもTeXが標準で画像を取り込む機能を持っていない原因)。
LaTeXではグラフィックスを扱うためのマクロが用意されているのですが、
このマクロのことは解説してない解説書が多いです。
 
画像を貼り付けるためにはgraphicxスタイルファイルを使います。
画像を貼り付けるためのマクロは<code>includegraphics</code>です。

```csharp
\usepackage{graphicx}

\includegraphics{test.bmp}
```


画像の表示の仕方はDVIウェアに依存します。
Windowsでよく使われているDVIウェアのDVIOUTでは、
susieプラグインを用いて表示しています。
susieプラグインさえあればPNGやPBMなどさまざまな形式の画像を表示できます。


## <a id="sec-generated-title-8"></a> <a id="color"></a>文章をカラーにしたい

colorスタイルファイルを使えばLaTeX文書をカラー化できます。

```csharp
\usepackage{color}
% 自分で色を定義することも出来る
\ifx\color@rgb\@undefined\else
  \definecolor{keyword}{rgb}{0,0,1}
  \definecolor{comment}{rgb}{0,0.5,0}
  \definecolor{string}{rgb}{0,0.5,0.5}
\fi

{\color{red}赤色の文字}
{\color{keyword}class}
{\color{string}"string"}
{\color{comment}/* comment */}
```



## <a id="sec-generated-title-9"></a> <a id="here"></a>図表をその場に表示したい

LaTeXでfigure環境やtable環境を使うと、適切と思われる位置に図表が勝手に移動されてしまいます。
まあ、そのほうがいいときも多いでしょうが、どうしても指定した位置に図表を表示したいときにはhereスタイルファイルを使いましょう。(here.styが必要)

```csharp
\usepackage{here}

\begin{figure}[H]
% 図
\end{figure}

\begin{table}[H]
% 表
\end{table}
```



## <a id="sec-generated-title-10"></a> <a id="ppt"></a>パワーポイントで書いた絵をLaTeXで使いたい

うちの学科では論文発表時のプレゼンはパワーポイントで作ります。
でも、論文書き自体はLaTeXで書くことが多いです。
そういうわけで、必然的にパワーポイントで絵を描いて、
それをLaTeXで書いた論文に入りつけたいという要望が生まれます。
 
でも、パワーポイントで描いた絵をBMPとかGIF, JPEGなどのビットマップ形式で保存すると、
LaTeXに貼り付けたときに画質が劣化します。
EPSみたいなベクタ形式で保存できればいいんですが、
あいにくパワーポイントではEPS出力は出来ません。
逆にパワーポイントの出力できるwmfとemfというベクタ形式はLaTeXの側が対応していません。
 
で、その解決策なんですが、一言で言っちゃうと、
wmfからEPSに変換するための「wmf2eps」というソフトがありますので、
パワーポイントで描いた絵をemfかwmf形式で保存して、
wmf2epsを使ってEPSに変換します。
この方法を使えばLaTeXに結構綺麗な図を張り込むことが出来ます。


## <a id="sec-generated-title-11"></a> <a id="omake"></a>おまけ

LaTeXのロゴ、HTMLでは「L<sup><small>A</small></sup>T<small>E</small>X」こう書くのが一般的なんでしょうか。
<code>L&lt;sup&gt;&lt;small&gt;A&lt;/small&gt;&lt;/sup&gt;T&lt;small&gt;E&lt;/small&gt;X</code>
