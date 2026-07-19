---
title: "Build 2015"
source_url: "https://ufcpp.net/blog/2015/5/build2015/"
content_type: "BlogEntry"
published_at: "2015-05-09T08:23:34"
updated_at: "2015-05-09T08:30:52"
tags: []
umbraco_id: 1704
parent_id: 1700
sort_order: 1
aliases: []
---

# Build 2015

[Build 2015][1] の資料に目を通すの数日かかった… 結構長し読みだったんですけども。

CEO交代後のマイクロソフトの

- デバイスとサービスの会社になる
- すべての開発者、すべてのデバイス、すべてのプラットフォームにアプローチ

という方針がついに形になって表れ始めたという印象でした。

## デバイス

発表の3分の1くらいは「Windows 10」がらみでした。

ようやく、デスクトップとモバイルの分断が解消されそうというか、WindowsとWindows Phoneが統合されそうというか。

### Universal

これまで 「ストア アプリ」とか変な名前だったけども、「Universal Windows Platform」「Universal Windows Apps」と、やっとまともな名前になった印象。

### 完全に単一のバイナリ

IoT、Phone、Tablet、デスクトップ、大スクリーン(XBoxゲーム、Surface Hubなどの会議システム)すべてをカバー。
(Windows 8.1の「Universal Apps」はWindowsとWindows Phoneは別バイナリ、ストアが単一化されてたのと、Shared Projectでのコード共有ができるようになってただけ)

そのために、

- レスポンシブ デザイン
  - スクリーン サイズに応じて Visual State Managerとか使ってXAMLのデザインを分岐
    - (XAML的にも、XAML中のタグを遅延ロードするような機能(x:DeferLoadStrategy)が増えてたり)
- API Contract
  - メソッド名や型名指定でAPIがデバイス上で使えるかどうかを調べて分岐する処理を書く

とかやるように。

## サービス

同じく発表の3分の1くらいは「Azure」がらみでした。といっても、OSとかハードウェア(IaaS的なクラウド)じゃなくて、「サービス」が主役。

- [Azure App Service][2]
  - API Apps: 開発者が新たにサービスAPIを作って公開・消費するための基板
  - Logic Apps: BizTalkやソーシャル関連APIなどののAPIを組み合わせてフローを構築するアプリ開発基盤
- [Azure Machine Leaning][3]
- [Office 365 Depelover Program][4]

などなど、細粒度のサービスをマイクロソフト自身が売ったり、サービスの開発・登録・検索・消費などをしやすくするための基板を提供したり。
マイクロソフトが自社の強みを活かしてクラウド戦略を推し進めてますね。

## すべての開発者に

.NET/C#を使える理由もだいぶ増えるし、使わなくていい理由もだいぶ増える印象。
いろんな言語・環境を使っている人が、フェアに相互に行き来できるよい環境ができそう。

### すべてのプラットフォームで.NET開発
- [Visual Studio Code][5]
- [.NET Core][6]
  - Windows x64環境向けはRCに
 Linux, Macはベータ
 - Xamarinは今、モバイル(Xamarin.iOS, Xamarin.Android)に注力していて、サーバー側機能はプロダクション環境向けのサポートしていない

Visual Studio Codeとか、中身は単に[Electron + Monaco][11]、つまり、これまでもVisual Studio Online向けに作っていた、Webなエディターを、Electron使ってアプリ化しただけ。
実装に関する手間は対してかかっていなさそうだし、名前に反して「Visual Studio」とは完全に別系統のアプリなんですが。
とはいえ、「Visual Studio」の名前を関したアプリとして.NET関連のどこかのチーム(たぶんASP.NET系な雰囲気がする)が提供・保守していくということの。

### Visual Studio利用者にすべてのプラットフォーム開発機能を提供
- [Cross-Platform Mobile Apps in C++][7]
- [Apache Cordova][8]
- [Xamarin][9]

コンパイラーが提供されているって意味では、C++は一番クロスプラットフォームなプログラミング言語のはずなんですけども。
とはいえ、プラットフォーム固有のAPI・ライブラリ参照であるとか、プロジェクト構築とか考えるとそんなに容易くない。
それが今回、クロスプラットフォーム向けのC++テンプレートが提供されて、Visual Studio上でAndroidやiOSアプリをC++でようになりました。

Office 2016がUniversal Apps化して、AndroidやiOS版もリリースされて、しかも、コードの9割以上を共有しているらしく、そのおこぼれな感じはします。
Officeの影響はやっぱり大きいですねぇ。Officeの性能を出すためにXAMLにも結構新機能が入ったそうですし。

### すべてのプラットフォーム開発者にUniversal Windows Appsの開発機能を提供
- [Universal Windows Platform Bridges][10]
  - Project "Astoria": 
  - Project "Islandwood": 
  - Project "Centennial": 
  - Project "Westminister": 

Bridges(相互の橋渡し)の各種プロジェクトは、コードネームがAndroidのA、iOSのI、ClassicのC、WebのWを頭文字とするワシントン州の地名なのかな、また。
レジストリの仮想化とか、Android/iOSのAPIの互換レイヤーの提供というものみたい。

これまで、スマホやタブレット対応のために一度AndroidやiOSに移っていったエンタープライズ企業をWindowsに呼び戻すよい戦略。
一方、コンシューマーだとどうだろう…という感じはします。

全APIの完全互換とかはたぶん無理だろうし、実際、Buildの資料にも「APIのサブセットを提供」って書かれてて。
「AndroidやiOSアプリのコードをほぼそのまま持ってきて、少し書き替えるだけでUniversal Windows Apps化できる」とか言っていますが、どこまで楽に移植できるのかはちょっと心配。
試してみたいものの、ダウンロードするのに登録が必要だったり、手持ちにそれなりの規模のAndroid/iOSアプリがないので。

  [1]: http://channel9.msdn.com/events/build/2015
  [2]: https://buchizo.wordpress.com/2015/03/25/azure-app-service/
  [3]: http://azure.microsoft.com/ja-jp/services/machine-learning/
  [4]: http://dev.office.com/
  [5]: https://code.visualstudio.com/
  [6]: https://github.com/dotnet/corefx
  [7]: https://msdn.microsoft.com/ja-jp/library/dn872463(v=vs.140).aspx
  [8]: https://msdn.microsoft.com/ja-jp/library/dn771545.aspx
  [9]: http://xamarin.com/
  [10]: https://dev.windows.com/en-us/uwp-bridges
  [11]: http://blog.shibayan.jp/entry/20150430/1430328999
