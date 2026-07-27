---
title: "Windows Presentation Foundation 概要（WPF）"
source_url: "https://ufcpp.net/study/dotnet/wpf/wpf_abst/"
content_type: "Article"
published_at: "2006-11-15T00:00:00"
updated_at: "2007-07-12T00:00:00"
tags: []
umbraco_id: 1394
parent_id: 1393
sort_order: 0
aliases:
  - "/study/dotnet/wpf_abst.html"
---

# Windows Presentation Foundation 概要（WPF）

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<strong id="wpf" class="keyword">Windows Presentation Foundation</strong> は .NET Framework 3.0 で追加された3つの主軸ライブラリの1つで、
非常に高機能な GUI 構築用ライブラリです。
<strong id="wpf0" class="keyword">WPF</strong> と略します。

最近の Flash を使ったウェブサイトみたいな、
ユーザの操作に反応してアニメーションしたり、
操作性や表現力に優れた見た目のアプリケーションを Rich Interactive Application、
略して RIA なんていうんですが、
WPF は、Windows アプリケーション・ウェブアプリケーション問わず、
RIA を開発するためのライブラリと位置づけられています。


## <a id="sec-generated-title-2"></a> <a id="feature"></a>特徴と利点

##### <a id="sec-generated-title-3"></a>GPU の恩恵が受けられる

今までの Windows アプリケーション開発用ライブラリ（GDI や GDI+ ）は、
レンダリング処理を CPU で行っていました。
グラフィック用のプロセッサ（GPU）のアクセラレーション機能の恩恵を受けようと思うと、
DirectX 等の特別なライブラリの利用が必須でした。

これに対して、WPF の GUI 描画は、（利用可能なら）GPU の能力をフルに活用します。
（要するに、DirectX の機能を吸収した。）

（ただし、
パフォーマンスよりは開発の容易さや拡張性重視で作られていて、
パフォーマンスが重要視される用途には向かないとのこと。
ゲーム制作などの用途には、やっぱり Managed Direct X や XNA を使って欲しい様子。）


##### <a id="sec-generated-title-4"></a>2D ビットマップ・ベクタグラフィック、3D グラフィック、動画像の統合

今までの Windows アプリケーション開発では、
2D ならGDI、3D なら Direct3D、動画なら DirectShow などと、
物によって設計方針の違う複数のライブラリを使う必要がありました。

WPF では、2D も 3D も、静止画も動画も、ビットマップもベクタグラフィックも、
全て統合していて、
プログラマは WPF という1つのライブラリでこれら全てを使えるようになりました。


##### <a id="sec-generated-title-5"></a>ビジュアルとロジックの分離

ビジュアルデザイン（GUI の見た目）とロジックデザイン（処理内容）は分離しろと言われるようになってもうずいぶんと経ちます。
（前者は美的センスが、後者は論理的なセンスが問われるため、
それぞれ別の人による分業をやりやすくしたい。）

HTML ＋ JavaScript を使ったインタラクティブなウェブページが流行ったのもこれが理由の1つで、
ビジュアルデザインは HTML で、
ロジックは JavaScript で書くというように、
ビジュアルとロジックの分離ができたからだと思います。

WPF では、ビジュアルデザインは XAML と呼ばれる XML ベースの言語を用いて行い、
ロジックは C# などのプログラミング言語で書くことができます。
前者向けには、Microsoft Expression Blend というビジュアルデザイナ向けのツールが提供される予定です
（2007/5 時点では、まだβ版）。
後者には、もちろん、Visual Studio を用いた開発が可能です。


##### <a id="sec-generated-title-6"></a>カスタマイズの柔軟性

WPF は非常に柔軟なカスタマイズ可能性を持っています。

たいていの GUI ライブラリでは、
ボタンなどのコントロールをカスタマイズしたいとき、
せいぜい背景色を変えるとか文字サイズを変えるくらいしかできないと思います。
中には、背景にグラデーションをかけるとか、
ボタンの角を丸く削るくらいはできる物もあるかもしれません。

WPF では、さらに、
テンプレート機能を使ってボタンを任意の形状に変えたり、
メディアブラシを使って背景に動画を表示したり、
かなり柔軟なカスタマイズが可能です。


## <a id="sec-generated-title-7"></a> <a id="element"></a>主な構成要素

* XAML（eXtensible Application Markup Language） … XML （HTML のようなタグ付け言語）を用いた GUI 構築

* GUI 部品のレイアウト機能（コンテナ）
    * Canvas … 座標を明示

    * StackPanel … 横一直線、もしくは、縦一直線に配置

    * WrapPanel … HTML のように、左詰（or 右詰）＆自動改行で配置

    * DockPanel … 子要素をパネルの上下左右・中央自在に配置

    * Grid … 行・列からなるグリッド状に配置



* 多彩な GUI 要素
    * System.Windows.Controls … テキストボックスやボタンなどのコントロール

    * System.Windows.Documents … 段落やリストなどを用いた、論理的な構造の文書

    * System.Windows.Shapes … 直線・矩形・円などの基本的な図形

    * System.Windows.Media … ドロー（ベクタ画像）、テキスト、音声・動画などを統合的したメディア機能
        * System.Windows.Media.Media3D … 3D メッシュなどを用いた3次元描写も可能





* ベクタグラフィックス … テキストボックスやボタンなどが全てベクタになっているので、 回転や拡大などが自由に行える



## <a id="sec-generated-title-8"></a> <a id="app"></a>アプリケーションの種類

##### <a id="sec-generated-title-9"></a>Windows アプリケーション

もちろん、Windows 上で動く GUI アプリケーション開発が可能です。

Windows アプリに関しては、制限なく全ての機能が利用可能。


##### <a id="sec-generated-title-10"></a>ブラウザアプリケーション

WPF では、
XBAP（XAML Browser APplication、エックスバップとか読むみたい）と呼ばれるウェブアプリケーション開発が可能です。

作り方はほとんど Windows アプリと同じですが、
ブラウザ中での実行を想定していて、
セキュリティ的な制限が掛かります。
セキュリティゾーンに関して、レベルの設定が可能です。


##### <a id="sec-generated-title-11"></a>Loose XAML

コードなしで未コンパイルの XAML もブラウザ中で実行可能で、
これを Loose XAML と呼びます。
Loose XAML だけでもかなり多彩なことを実現可能です。


##### <a id="sec-generated-title-12"></a>MIME Type

ちなみに、ウェブサーバ上に Loose XAML や XBAP をアップロードして、
ブラウザ越しに実行するためには、
ウェブサーバの MIME 設定が必要になります。
（でないと、SecurityException がスローされる。）

<table summary="MIME Type">
	<caption>
		MIME Type
	</caption>
	<tr>
		<th>拡張子</th>
		<th>MIME Type</th>
	</tr>
	<tr>
		<td markdown="1">.manifest</td>
		<td markdown="1">application/manifest</td>
	</tr>
	<tr>
		<td markdown="1">.xaml</td>
		<td markdown="1">application/xaml+xml</td>
	</tr>
	<tr>
		<td markdown="1">.application</td>
		<td markdown="1">application/x-ms-application</td>
	</tr>
	<tr>
		<td markdown="1">.xbap</td>
		<td markdown="1">application/x-ms-xbap</td>
	</tr>
	<tr>
		<td markdown="1">.deploy</td>
		<td markdown="1">application/octet-stream</td>
	</tr>
	<tr>
		<td markdown="1">.xps</td>
		<td markdown="1">application/vnd.ms-xpsdocument</td>
	</tr>
</table>


Windows サーバの場合、サーバに .NET Framework 3.0 が入っていれば、
特に手動で設定する必要はなさげ。
Apache の場合、.htaccess に、例えば以下のような記述を追加すれば OK。

```apache
AddType application/xaml+xml xaml
AddType application/x-ms-xbap xbap
```



## <a id="sec-generated-title-13"></a> <a id="compare"></a>他のプレゼンテーション層技術との比較

WPF を使ったアプリケーションにしても Windows アプリと XBAP がありますし、
WPF とコンセプトの近いアプリケーション構築技術として、Silverlight や ASP.NET＋AJAX などがあります。

コンセプトが近いということもあって、
どれをいつ使えばいいのか分かりにくかったりもするんで、
それぞれの特徴を表にしてまとめてみます。

<table summary="Microsoft のプレゼンテーション層技術">
	<caption>
		Microsoft のプレゼンテーション層技術
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>クライアント</th>
		<th>配置展開方法</th>
		<th>用途・利用場面</th>
	</tr>
	<tr>
		<th>WPF</th>
		<td markdown="1">Windows XP SP2、Vista</td>
		<td markdown="1">ダウンロードしてから実行。あるいは、ClickOnce などの配布・更新技術を使う。</td>
		<td markdown="1">Windows の機能をフルに使いたい場合。クライアント上のファイルにフルにアクセスしたい場合。</td>
	</tr>
	<tr>
		<th>WPF XBAP</th>
		<td markdown="1">IE ＋ XP SP2、Vista</td>
		<td markdown="1">IE 上で実行。</td>
		<td markdown="1">Windows のみがターゲットのブラウザアプリケーション。</td>
	</tr>
	<tr>
		<th>Silverlight</th>
		<td markdown="1">IE、FireFox、Safari</td>
		<td markdown="1">ブラウザのプラグインで実行。</td>
		<td markdown="1">IE 以外もターゲットにした RIA。</td>
	</tr>
	<tr>
		<th>ASP.NET</th>
		<td markdown="1">任意のウェブブラウザ</td>
		<td markdown="1">サーバ上で HTML 化したものをクライアントに送信。ウェブページとして表示。</td>
		<td markdown="1">より多くの環境をターゲットにしたい場合。</td>
	</tr>
</table>


ただし、Windows XP 上で WPF アプリを動かすためには .NET Framework 3.0 ランタイム（32bit版で50MB、64bit版で90MB）のインストールが必要。（Vista には標準でインストールされている。）

Windows 2000 以前の Windows OS は、
残念ながら .NET Framework 3.0 のサポート対象外なので、
2000 もターゲットにしたい場合には、WPF ではなくて Windows Forms を使います。

また、ASP.NET と他の3者は必ずしも競合するものではなくて、
相補的なものです。
Silverlight を使った ASP.NET ウェブコントロール（クライアント側のブラウザ上では Silverlight として表示される）を作ったりもできますし、
クライアントサイド WPF アプリケーションから ASP.NET ウェブサービスを呼び出してサーバ上での処理を行ったりもできます。
