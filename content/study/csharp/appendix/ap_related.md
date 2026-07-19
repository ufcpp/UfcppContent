---
title: "関連技術"
source_url: "https://ufcpp.net/study/csharp/appendix/ap_related/"
content_type: "Article"
published_at: "2007-06-24T00:00:00"
updated_at: "2008-06-28T00:00:00"
tags: []
umbraco_id: 1379
parent_id: 1377
sort_order: 1
aliases:
  - "/csharp/ap_related"
  - "/csharp/ap_related.html"
  - "/csharp/appendix/ap_related/"
  - "/study/csharp/ap_related"
  - "/study/csharp/ap_related.html"
---

# 関連技術

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# を使う開発フレームワーク・ツール・ライブラリの説明を少し。

C# のみ・標準ライブラリのみでもかなり充実したことができますが、
Microsoft は .NET Framework をコアに色々なフレームワークを構築したりしていて、
その辺りを覚えるとさらに高度な開発が可能です。


## <a id="sec-generated-title-2"></a> <a id="aspx"></a>ASP.NET

Microsoft が開発したサーバサイトアプリケーションフレームワークです。
HTML 中にコードを埋め込んだり（インラインコード）、
「HTML ＋ ソースファイル」（コードビハインド）でウェブアプリケーション開発を行います。

別ページにて、簡単な説明を書き始めました：「[クラスライブラリ](../../dotnet/index.md)」。


##### <a id="sec-generated-title-3"></a>リンク

<!-- 以下、reflinks 要素の変換後の出力です。参照先に同じリンクが記述されている可能性があります。-->

[ASP.NET Developer Center](http://www.microsoft.com/japan/msdn/asp.net/)
: 日本語公式サイト。

[ASP.NET クイック スタート チュートリアル](http://ja.gotdotnet.com/quickstart/aspplus/)
: Microsoft が提供するチュートリアル。 Microsoft はドキュメントの多言語対応に結構力をいれているので、 日本語的にも割りと安心して読めると思います。

[＠IT 連載　 プログラミングASP.NET](http://www.atmarkit.co.jp/fdotnet/aspnet/index/index.html)
: こちらは＠ITの記事。



## <a id="sec-generated-title-4"></a> <a id="speech"></a>音声合成・認識

.NET Framework 3.0 でひそかに音声合成・認識機能が追加されていたりします。
.NET Framework 3.0 といえば WPF, WCF, WF, Card Space の4つが主要機能で、
それに隠れがちですが、
実は System.Speech 名前空間というのも追加されています。

Windows XP 以降には音声合成・認識機能が付いているわけですが、
それを使ったプログラミング API（Speech API、略して SAPI）ってのがありました。
System.Speech は、それの .NET 版です。

ただし、現状では、日本語の音声合成・認識エンジンは標準搭載されていないようです。
（別途、日本語エンジンをインストールすれば、System.Speech を通した利用も可能。）


##### <a id="sec-generated-title-5"></a>リンク

[Windows Presentation Foundation Station、Speech_Synthesis_Top](http://www.vbstation.net/wpf/index.php/Speech_Synthesis_Top)
: 主に WPF の解説をしているページですが、 System.Speech への言及もあり。



## <a id="sec-generated-title-6"></a> <a id="robotics"></a>MS Robotics Studio

.NET Framework でロボット制御プログラムの開発ができます。

2004・5年ごろから、LEGO mindstorms や、近藤科学 KHR-1 など、
安価で手に入るロボットキットが色々登場しているわけですが、
MSRS はいろんなロボットキットに対して利用可能。
それぞれの制御方式の違いを、ライブラリが吸収してくれています。
そのおかげで、
mindstorms と KHR-1 の協調動作なんかも可能。

C# や VB を使えるのはもちろんのこと、
加えて、
ビジュアルプログラミングツール（Microsoft Visual Programing Language (VPL)）付き。

ロボット工学は日本でもかなり盛り上がっている分野で、
この Robotics Studio は MS 日本法人の開発者も多分に貢献しているみたい。

ロボット制御に興味のある人はもちろん、
ロボットなんてどうでもいいという人にとっても面白そうな技術かも。
というのも、ロボットと言うのは大量のセンサーやアクチュエータの集合体で、
それぞれのパーツが分散動作しているわけで、
全身の制御にはコンカレント（並列）プログラミングが必要になる。
GUI の登場によって（GUI 開発と相性の良かった）オブジェクト指向プログラミングが一気に普及したのと同様に、
ロボット制御によって新しいパラダイムが生まれるかもしれない。


##### <a id="sec-generated-title-7"></a>リンク

[Microsoft Robotics Studioメモ](http://www.saturn.dti.ne.jp/~npaka/robotics/index.html)
: 日本語のまとめサイトというとここ。

[解説！ ロボット開発環境Robotics Studio](http://www.atmarkit.co.jp/fembedded/index/robotics.html)
: ＠IT 連載記事。

[MSDN, Microsoft Robotics Studio](http://msdn2.microsoft.com/robotics/)
: 公式サイト。英語のみ。



## <a id="sec-generated-title-8"></a> <a id="xna"></a>XNA Game Studio

C# （に限らず、.NET Framework 対応言語）でゲーム開発。

XBOX 360 用ゲームと Windows 用ゲームが同じフレームワーク・ツールで開発可能です。
（まったく同じソースで XBOX 360 / Windows 両対応とはいかないまでも、ほとんどコピーでできる。）

XBOX 360 用ゲーム開発では、
年間税別9,800円のライセンス料と、
XBOX 360 本体と XNA Game Studio だけあれば実機動作可能。
作ったゲームを XBOX Live や Windows Live を通してネット配布することも可能。


##### <a id="sec-generated-title-9"></a>リンク

[XNA Game Studioメモ](http://www.saturn.dti.ne.jp/~npaka/xna/index.html)
: まとめサイト。

[MSDN, XNA デベロッパー センター](http://www.microsoft.com/japan/msdn/xna/)
: 日本語公式サイト。



## <a id="sec-generated-title-10"></a> <a id="silverlight"></a>Silverlight

Microsoft の Adobe Flash 対抗技術。

「[XAML](../../dotnet/wpf/wpf_xaml.md#xaml)」 と JavaScript を用いて作るので、
昔の Flash と違って、（やろうと思えば）テキストエディタで手書き可能。
（まあ、普通は、Microsoft から提供されている Expression というツールか Visual Studio を使って作ることになると思います。）

Silverlight 1.0 が JavaScript だったのに対して、
Silverlitht 2 では .NET Framework のサブセットを搭載して、
C# や VB.NET での開発が出来るます。
Silverlight は 2 からが本番という説も。

「XAML ＋ コードビハインドでリッチでインタラクティブなアプリ（RIA）開発」という方向性は
「[Windows Presentation Foundation](../../dotnet/wpf/wpf_abst.md#wpf)」と共通。
完全互換とはいかないものの、WPF とかなり近い書き方ができます。
ただし、Silverlight は WPF のサブセットとなっていて、
機能面にはかなり制限があります。
（それでも、アニメーションや動画ファイルの再生など、RIA 開発には十分な機能は揃ってる。）

具体的には、
Silverlight に標準で用意されているコントロールは Canvas, Ellipse, Glyphs, Image, InkPresenter, Line, MediaElement, Path, Polygon, Polyline, Rectangle, TextBlock くらい。
ボタンやリストボックスもないので、
その辺りが使いたければ Rectangle とか TextBlock などを使って自分でそれっぽく配置する必要あり。
（Silverlight 2 には UI controls package ってのが付いていて、
ボタンなんかも使える。）


##### <a id="sec-generated-title-11"></a>リンク

[Microsoft Silverlight](http://www.microsoft.com/japan/silverlight/)
: 日本語公式サイト。 英語版 → http://silverlight.net/

[Microsoft Expression](http://www.microsoft.com/japan/products/expression/default.mspx)
: Microsoft のデザインツール Expression の公式サイト。 Silverlight と関係しそうなのは Web（ウェブページデザイン）とか Blend（アニメーション作成）辺りだと思う。



## <a id="sec-generated-title-12"></a> <a id="dlr"></a>Dynamic Language Runtime

.NET Framework では、基盤となるランタイム環境（CLR: Common Language Runtime）を用意することで、
プログラミング言語の壁を越えた開発が可能になっています。
（例えば、VB.NET で作ったクラスを C# で継承して、それをさらに C++/CLI から利用するというようなことが可能。）

で、そういう共通ランタイムの動的言語（Python とか Ruby みたいなやつ）版を、
.NET Framework の上に作りたいみたい。
（2008年6月時点では、Silverlight 2 のβ版に同梱されているのと、IronPython βとして配布されています。）

これが正式にリリースされれば、
JavaScript で書かれたライブラリを Ruby から利用するとか、
動的言語間の連携がスムーズにできるようになるはず。


## <a id="sec-generated-title-13"></a> <a id="powershell"></a>PowerShell

Windows の新しいシェル環境。

コマンドプロンプトなどでは、あるコマンドの出力を別のコマンドの入力に与えたい場合、
一度、文字列で出力して、受け取った側がその文字列を解析してデータに戻すというような処理が必要でした。

これに対して、PowerShell では、
コマンド間で、.NET Framework のオブジェクトをそのまま受け渡し可能です。

解説ページ（サイト内）：「[Windows PowerShell](../../powershell/index.md)」


##### <a id="sec-generated-title-14"></a>リンク

<!-- 以下、reflinks 要素の変換後の出力です。参照先に同じリンクが記述されている可能性があります。-->

[Windows PowerShell でのスクリプティング](http://www.microsoft.com/japan/technet/scriptcenter/hubs/msh.mspx)
: 日本語公式サイト。

[次世代Windowsシェル「Windows PowerShell」を試す](http://www.atmarkit.co.jp/fdotnet/special/powershell01/powershell01_01.html)
: ＠IT の特集記事。

[PowerShell FAQ](http://newpops.wankuma.com/)
: PowerShell 関連書籍「PowerShell 宣言！」の著者のサイト「PowerShell Memo」から、 PowerShell の Tips を抽出して整理したもの。
