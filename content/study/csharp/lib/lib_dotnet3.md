---
title: ".NET Framework 3.0 / 3.5"
source_url: "https://ufcpp.net/study/csharp/lib/lib_dotnet3/"
content_type: "Article"
published_at: "2007-07-07T00:00:00"
updated_at: "2008-06-28T00:00:00"
tags: []
umbraco_id: 1357
parent_id: 1350
sort_order: 5
aliases:
  - "/csharp/lib/lib_dotnet3/"
  - "/csharp/lib_dotnet3"
  - "/csharp/lib_dotnet3.html"
  - "/study/csharp/lib_dotnet3"
  - "/study/csharp/lib_dotnet3.html"
---

# .NET Framework 3.0 / 3.5

##<a id="sec-generated-title-1"></a> <a id="abst"></a>.NET Framework 3.0 概要
.NET Framework 3.0 といいつつ、
Common Language Runtime （.NET アプリを動作させるための実行エンジン）自体は .NET Framework 2.0 のままで、
ライブラリコンポーネントが追加されたもの。
（参考：「[クラスライブラリ](../../dotnet/index.md)」）

.NET Framework 3.0 で追加された主要なコンポーネントは以下の4つです。

* 「[Windows Presentation Foundation](../../dotnet/index.md#wpf)」

* 「[Windows Communication Foundation](../../dotnet/index.md#wcf)」

* Windows Communication Foundation

* Windows CardSpace


その他、音声認識・合成ライブラリなど、地味にいろいろと追加されています。


##<a id="sec-generated-title-2"></a> <a id="abst"></a>.NET Framework 3.5 概要
.NET Framework 3.5 では、C# のバージョンが 3.0 となり、
LINQ クエリ式などの構文が追加されました。
.NET Framework 3.5 のライブラリも、LINQ 関連の追加が多くなっています。

その他にも、たとえば以下のようなコンポーネントが追加されています。

* ASP.NET AJAX

* アドイン開発ライブラリ



##<a id="sec-generated-title-3"></a> <a id="abst"></a>.NET Framework 4.0 概要
Visual Studio 2010 と同時期に正式版リリース予定の .NET Framework 4.0 では、
かなりいろいろとライブラリが追加される模様。

Microsoft も、最近はいきなりまとめて製品版をリリースするんじゃなくて、
パーツごとに開発して（中にはオープンソースにしている物も多々）、
最終的に .NET Framework に統合するスタイルをとることが多いです。
.NET Framework 4.0 で追加される予定のものも、
2008年ごろから少しづつβ版が公開されています。

* 「[Windows Presentation Foundation](../../dotnet/wpf/wpf_abst.md#wpf)」への機能追加
    * [WPF Toolkit](http://www.codeplex.com/wpf)

    * [WPF Ribbon](http://www.codeplex.com/wpf/Wiki/View.aspx?title=WPF%20Ribbon%20Preview)



* [Dynamic Language Runtime](http://www.codeplex.com/dlr)（DLR）
    * 動的言語向け共通プラットフォーム



* [Managed Extensibility Framework](http://www.codeplex.com/MEF)（MEF）
    * プラグイン用ライブラリ



* Parallel Extensions （[June 2008 CTP](http://www.microsoft.com/downloads/details.aspx?FamilyId=348F73FD-593D-4B3C-B055-694C50D2B0F3&amp;displaylang=en)）（[開発ブログ](http://blogs.msdn.com/pfxteam/)）
    * 並列処理ライブラリ



* Velocity （[CTP2](http://www.microsoft.com/downloads/details.aspx?FamilyId=B24C3708-EEFF-4055-A867-19B5851E7CD2&amp;displaylang=en)）（[開発ブログ](http://blogs.msdn.com/velocity/)）
    * 分散キャッシュライブラリ
