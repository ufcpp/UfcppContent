---
title: "Windows 10アプリ開発に関する近況 (Astoria開発中止とか)"
source_url: "https://ufcpp.net/blog/2016/2/an-update-on-the-developer-opportunity-and-windows-10/"
content_type: "BlogEntry"
published_at: "2016-02-26T02:34:09"
updated_at: "2016-02-26T02:39:05"
tags: []
umbraco_id: 1876
parent_id: 1873
sort_order: 2
aliases: []
---

# Windows 10アプリ開発に関する近況 (Astoria開発中止とか)

以下のブログがちょっと話題になっていますが。

[An Update on the Developer Opportunity and Windows 10](https://blogs.windows.com/buildingapps/2016/02/25/an-update-on-the-developer-opportunity-and-windows-10/)

Windows 10アプリ開発に関する近況的な話で、Xamarinとか各種ブリッジ(他の環境向けアプリからの移植)とかの現状を説明しているんですが。

[Xamarin買収の話](http://www.publickey1.jp/blog/16/xamariniosandroidvisual_studio.html)と同時期に出したせいとか、ブログの内容がいまいちとか色々あり… なんか変な印象になってるなぁとか、ちょっと不安な感じ。文章読んでて受ける印象的に、これ、そんなに技術に詳しくない広報の人が、技術者から聞いた内容を元に書き起こしてて、微妙に正確じゃない表現になってるんじゃないかなぁとか思ったり(Windowsチームはそれをやらかしそう、という偏見もあり)。

## Bridge for Android (コードネーム「Astoria」)の開発停止

この記事、「[マイクロソフトはBridge for Android (Astoria)の開発停止を認める](http://www.androidheadlines.com/2016/02/microsoft-confirms-closure-of-bridge-for-android-apps-astoria.html)」とかいう話題にされてしまっていて。それはこのブログの本題じゃなかったと思うんだけどなぁ…

まあ、[去年の11月にはそういう話が出ていた](http://japan.cnet.com/news/service/35073589/)んですが、
確かに、ブログで明示的に停止した話が書かれたのは初めてなのかな…
そのせいで、なんか騒動になっているみたいで。

ですが、
Xamarin買収のタイミングで変な文章を書いたので「AstoriaとXamarin、2つは必要ない」という主張だと誤認されたり、
Bridge for iOSとの関連の説明が下手なせいで「ブリッジ技術にAndroid向けとiOS向けの2つは必要ない」と取れる文章だったり、
なんか変な感じ(なので、これ、非技術者な広報担当者が書いてるなぁという印象です)。

## クロス開発と、ブリッジと

そもそも、クロス開発と各種ブリッジは相補的というか、逆のことをやっているというか。

クロス開発は、これからスマホ向けアプリを作ろうとする人に、UWP (Windows 10 Mobile)も選択肢に入れてもらう(iOS、Android、Windows全部をターゲットにしてもらう)ためには何を使ってほしいかという話。マイクロソフトが提供する選択肢としては、

- [Visual C++ Cross-Platform Mobile](https://www.visualstudio.com/en-us/features/cplusplus-mdd-vs.aspx)
  - C++でクロス開発するためのプロジェクト テンプレートとかデバッガーとか
  - 今、Android開発に関してはJavaにも対応してたり
- [Apache Cordova](https://cordova.apache.org/)と、その[Visual Studio対応](https://codeiq.jp/magazine/2015/06/24809/)
  - JavaScriptでWeb的に(UIもHTMLで書いて)ネイティブ アプリ化する
- [Xamarin](https://xamarin.com/)
  - Mono ベース/C# でクロス開発

とかがあったわけです。

一方、ブリッジは、特定環境向けに作られた既存のアプリをUWP化してもらうための技術。開発が続いてるのは、

- [Web Bridge](http://microsoftedge.github.io/WebAppsDocs/en-US/win10/HWA.htm) (コードネーム Westminster)
  - Web技術をそのままUWP化
- Win32/デスクトップ向け.NET移植 (コードネーム Centennial)
  - デスクトップ アプリをサンドボックス動作させるだけっぽい
- [Bridge for iOS](https://dev.windows.com/bridges/ios) (コードネーム Islandwood)
  - Objective-Cで書かれたiOS向けコードをビルドして、UWP化できる機能

など。

で、今回、開発停止が正式になったというのが、以下のAstroia

- コードネーム Astoria
  - Windows 10にAndroid互換レイヤー(JVM + Android API)を載せて、エミュレーションしようという技術
  - ここが Bridge for iOSとの差。ソースコード レベルでの移植じゃなくて、バイナリをそのまま動かそうとしてた

## Java for Android対応

一方で、今のVisual Studioって、JavaでのAndroidアプリ開発には実は対応していたり。

- [Java support in Visual Studio for Android](https://blogs.msdn.microsoft.com/vcblog/2015/11/06/java-debugging-and-language-support-in-visual-studio-for-android/)
  - Javaで書かれたAndroid向けコードをビルドして、Androidアプリをビルド・デバッグ

これ、しかもちょっと面白いのが、C++チームが保守してる(上記ブログはC++チームが書いたもの)という事実。

今、マイクロソフト内のC++チームが負ってる債務は「クロス開発」のようです。
その結果、Android開発はC++チームの範疇で、Java for Android対応もC++チームの債務。

という背景から、Androidからのブリッジも、Bridge for iOS (Objective-Cからのビルド)と同じく、JavaソースコードからUWPを作る機能であるべきなんじゃないかなぁという感じ。

で、実際、Astoriaの開発停止の話が出たのって、この[Java support in Visual Studio for Android](https://blogs.msdn.microsoft.com/vcblog/2015/11/06/java-debugging-and-language-support-in-visual-studio-for-android/)が出たのと同時期だったりします。

## 結局のところAstoria開発停止の理由

実際のところは中の人に聞いてみないとわからないものの、僕が受けた印象としては、以下のような話だったんじゃないかなぁと。

- Xamarinとの関連
  - 「Xamarin買収の話題をきっかけに、いったん現状報告しとくか」程度であって、Astoria開発停止とは完全に無関係
- Bridge for iOS との関連
  - 本来いうべきこと: Bridge for iOS同様、ソースコードからのビルドであるべきではないか。互換レイヤー提供によるエミュレーションと、ソースコードからのビルドの2方式あるのは混乱の元
  - ブログの文面: Bridge for iOSに注力する。2つのブリッジ技術があるのは混乱の元
- 実際あり得そうな本来のAstoria開発停止理由
  - [Java support in Visual Studio for Android](https://blogs.msdn.microsoft.com/vcblog/2015/11/06/java-debugging-and-language-support-in-visual-studio-for-android/) をベースに、Javaソースコード レベルでUWP化に対応するのが筋
  - ここが「計画としてはあるけどまだめどが立ってない、あるいは、//build/辺りでの発表目指してる」とかで今は何も言えない
       - 言えない(というか、できるかどうかもわかんない)ことをお茶を濁した結果がブログの微妙な文面

伝言ゲームで文面が変になったり、言えないことをお茶を濁して余計言っちゃいけない感じの文章になったり、まあ、割かしよくあることだと思います…
