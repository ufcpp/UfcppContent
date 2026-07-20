---
title: "実行基盤"
source_url: "https://ufcpp.net/study/csharp/framework/fwinfrastructure/"
content_type: "Article"
published_at: "2013-04-07T00:00:00"
updated_at: "2015-05-06T14:12:42"
tags: []
umbraco_id: 1345
parent_id: 1344
sort_order: 0
aliases:
  - "/csharp/FwInfrastructure"
  - "/csharp/FwInfrastructure.html"
  - "/csharp/framework/fwinfrastructure/"
  - "/study/csharp/FwInfrastructure"
  - "/study/csharp/FwInfrastructure.html"
---

# 実行基盤

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

（書きかけ）

MS 実装の名前「.NET Framework」、あるいは、標準規格の名前「CLI（Common Language Infrastructure）」とは。

* 中間言語（IL: Intermediate Language）仕様と、その中間言語を生成するコンパイラー

* 中間言語を解釈して実行する下層実行システム（VES: Virtual Execution System）

* 基礎ライブラリ（BCL: Base Class Library）



## <a id="sec-generated-title-2"></a> <a id="cli"></a>CLI

IL（メタデータ含めて）、VES、BCL それぞれの意味合いを

共通型システム

<figure>

[![CLI](../../../../assets/media/ufcpp2000/csharp/fig/Framework/cli.png)](../../../../assets/media/ufcpp2000/csharp/fig/Framework/cli.png)

<figcaption>CLI</figcaption>
</figure>



## <a id="sec-generated-title-3"></a> <a id="android"></a>余談: Android は Java か？

とか言われる理由。
要は、「Java 言語だけど、実行システムもライブラリも Java じゃない」。

<figure>

[![Java と Android](../../../../assets/media/ufcpp2000/csharp/fig/Framework/android.png)](../../../../assets/media/ufcpp2000/csharp/fig/Framework/android.png)

<figcaption>Java と Android</figcaption>
</figure>


<figure>

[![Java と Android](../../../../assets/media/ufcpp2000/csharp/fig/Framework/android2.png)](../../../../assets/media/ufcpp2000/csharp/fig/Framework/android2.png)

<figcaption>Java と Android</figcaption>
</figure>


Write once, run anywhere しようと思ったら全部一致してないとダメ。
Java を名乗るためには互換性テストに通らないといけない。
「Java 言語だけど Java じゃない」状態。

これに対して、 .NET は？

<figure>

[![.NET](../../../../assets/media/ufcpp2000/csharp/fig/Framework/dotnet.png)](../../../../assets/media/ufcpp2000/csharp/fig/Framework/dotnet.png)

<figcaption>.NET</figcaption>
</figure>


<figure>

[![.NET](../../../../assets/media/ufcpp2000/csharp/fig/Framework/dotnet2.png)](../../../../assets/media/ufcpp2000/csharp/fig/Framework/dotnet2.png)

<figcaption>.NET</figcaption>
</figure>
