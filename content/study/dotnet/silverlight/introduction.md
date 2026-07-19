---
title: "Silverlight とは"
source_url: "https://ufcpp.net/study/dotnet/silverlight/introduction/"
content_type: "Article"
published_at: "2010-03-28T00:00:00"
updated_at: "2015-05-06T14:14:57"
tags: []
umbraco_id: 1412
parent_id: 1411
sort_order: 0
aliases:
  - "/dotnet/silverlight/introduction/"
  - "/silverlight/introduction"
  - "/silverlight/introduction.html"
  - "/study/silverlight/introduction"
  - "/study/silverlight/introduction.html"
---

# Silverlight とは

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

Silverlight は Microsoft が提供する RIA 環境です。


## <a id="sec-generated-title-2"></a> <a id="position"></a>Silverlight の位置付け

##### <a id="sec-generated-title-3"></a>RIA 開発環境

インターネットの普及からもうずいぶんと経ち、
今では多くのアプリケーションがサーバー上で実行されるようになりました。
とはいえ、クライアント側での処理が一切必要ないかというとそうでもなくて、
リッチな UI（ユーザーインターフェース）の実現にはクライアント側での処理が必ず残ります。

このような、リッチな UI を持つウェブアプリケーションのことを
RIA（Rich Internet Application）と呼びます。
あるいは、意味を広げて、
ウェブに限らずリッチな UI を持つアプリケーション全般を指して RIA（Rich Intaractive Application）と呼びます。

Silverlight は、
既存の Windows アプリ（「[WPF](../wpf/wpf_abst.md#wpf0)」）とほぼ同様の開発モデルを用いて RIA 開発を行うための技術です。


##### <a id="sec-generated-title-4"></a>クロスプラットフォーム

一般に、RIA は HTML やブラウザープラグインを用いて、
ブラウザー上で実行されます。
Silverlight も、ブラウザープラグインとして提供されていて、ブラウザー上で動くアプリを作ることができます。

Silverlight は Microsoft の技術ですが、Windows/IE に限らず、さまざまな OS やブラウザーに対応しています。
例えば、2010年現在、Microsoft が公式にサポートするものだけで、

* OS: Windows（XP 以降）、Mac OS（10.4.8 以降）

* ブラウザー: IE（6 以降）、Safari（3, 4）、Firefox（2, 3）


に対応しています。
また、Novell 社が中心となって開発している Silverlight 互換製品である Moonlight によって、Linux にも対応しています。

今後は、PC だけでなく、携帯電話上でも Silverlight が動くようになります。
（Windows Phone 7 のアプリ開発は Silverlight で行います。
また、2010年現在、Silverlight 2 相当のようですが、Nokia の Symbian 端末向け Silverlight が提供されています。）


##### <a id="sec-generated-title-5"></a>3 screens and a cloud の中核技術

「クラウド」という単語が流行るに至った背景には、
非常に多くアプリが「インターネットの向こう側」で実行されるようになったというものがあります。
ただ、RIA の説明でも書きましたが、
クライアント側でも、リッチな UI を表示するという処理が残ります。

このような背景の中、
Microsoft は 3 screens and a cloud（3つの画面とクラウド）という戦略を掲げています。
3つのスクリーンとは、PC、テレビ、携帯電話のことを指します。
すなわち、3 screens and a cloud の目標は、
インターネット上で動くサービスに対して、
これら3種類の機器で可能な限り同じユーザー体験をを提供することです。

Windows Phone 7 アプリの開発環境は、Silverlight もしくは XNA（C# ゲーム開発フレームワーク）の2つになります。
Windows Phone 7 版の Silverlight は、
PC 版と比べると機能に一部制限があるものの、
ソースコードの大部分を PC 版と共有できます。
すなわち、Silverlight でアプリ開発を行えば、将来的に Windows Phone 7 向けの RIA にすることができます。


## <a id="sec-generated-title-6"></a> <a id="user"></a>ユーザー視点から見た Silverlight

ユーザー視点から見ると、Silverlight には以下のような利点があります。


##### <a id="sec-generated-title-7"></a>RIA

位置付けでも述べたように、Silverlight は RIA 用フレームワークです。
RIA は、下手すると過剰装飾にもなりがちですが、
適切に使えば非常に使い勝手のいいアプリを構築できます。


##### <a id="sec-generated-title-8"></a>ディジタル著作権管理（DRM）

Silverlight は動画像の DRM に対応していて、
商用の動画コンテンツが数多く提供されています。
Silverlight なら、Windows でも Mac でも DRM 付き動画を視聴することができます。


##### <a id="sec-generated-title-9"></a>アクセシビリティ

RIA ではアクセシビリティ（視覚障害者などでもアクセスできるかどうか）に問題が出がちですが、
Silverlight では音声読み上げに対応するなど、アクセシビリティにも取り組んでいます。


## <a id="sec-generated-title-10"></a> <a id="dev"></a>開発者視点から見た Silverlight

Silverlight は、
ユーザー視点以上に、開発者視点での利点が非常に魅力的です。


##### <a id="sec-generated-title-11"></a>C#/.NET Framework が利用できる

WPF と同モデル：
デスクトップアプリ開発用のフレームワークである 「[WPF](../wpf/wpf_abst.md#wpf0)」 とほぼ同様のモデルで RIA を開発できます。

サーバーもクライアントも C#：
.NET Framework がカバーする範囲は非常に広く、
ウェブアプリ（ASP.NET）、ウェブサービス（WCF）、データベースアクセス（LINQ）など、一通りのものがそろっています。
サーバー側のコードとクライアント側のコードで別の言語を覚える必要がなく、
全て C# だけで開発できます。


##### <a id="sec-generated-title-12"></a>Visual Studio が利用できる

Windows アプリ開発環境として、歴史も古く、利用者も多い Visual Studio を使って RIA 開発ができます。
プロジェクトテンプレート、コード補完、単体テスト、ステップイン実行デバッグなど、開発を支援するさまざまな機能を持っています。
また、Team Foundation Server も併用することで、
ソースコード管理、作業項目管理、継続的インテグレーションも行えます。


##### <a id="sec-generated-title-13"></a>視覚的なデザインとロジックの分離

Silverlight は、視覚デザイン面を担当するデザイナーと、ロジック実装を担当する開発者が協業しやすいようにフレームワークが作られています。


##### <a id="sec-generated-title-14"></a>柔軟な UI 開発

Silverlight では、非常に柔軟な UI を作れます。
例えば、ボタンなどのコントロールを自由に拡大・縮小・回転させたり、コントロール内で動画を再生したりすることができます。
また、Silverlight では、ビットマップ画像、ベクター画像、動画像など、さまざまなメディアを統一的な記述で表示できます。

柔軟な UI を「原理的に作成可能」なだけでなく、
Silverlight では、標準でさまざまなコントロールが用意されています。


## <a id="sec-generated-title-15"></a> <a id="link"></a>関連リンク

[公式サイト（英語）](http://www.silverlight.net/)
: 

[Microsoft による機能紹介（日本語）](http://www.microsoft.com/japan/silverlight/)
: 

[開発者向け公式サイト（日本語）](http://msdn.microsoft.com/ja-jp/silverlight/default.aspx)
:
