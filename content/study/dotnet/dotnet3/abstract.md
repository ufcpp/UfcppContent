---
title: ".NET Framework 概要"
source_url: "https://ufcpp.net/study/dotnet/dotnet3/abstract/"
content_type: "Article"
published_at: "2007-07-07T00:00:00"
updated_at: "2015-05-06T14:14:17"
tags: []
umbraco_id: 1392
parent_id: 1391
sort_order: 0
aliases:
  - "/dotnet/abstract"
  - "/dotnet/abstract.html"
  - "/dotnet/dotnet3/abstract/"
  - "/study/dotnet/abstract"
  - "/study/dotnet/abstract.html"
---

# .NET Framework 概要

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

.NET Framework 2.0 では、「[ジェネリック](../../csharp/oop/sp2_generics.md#generics)」への対応など、言語拡張が主でしたが、
3.0 以降、ライブラリに大幅な強化が加えられています。


## <a id="sec-generated-title-2"></a> <a id="dotnet3"></a>.NET Framework 3.0

.NET Framework 3.0 では WPF、WCF、WF、WCS と呼ばれる新フレームワーク（ライブラリ）が追加されました。

* Windows Presentation Foundation（<strong id="wpf" class="keyword">WPF</strong>）… 高機能 GUI フレームワーク。2D / 3D、ビットマップ/ベクタグラフィック、動画・音声などを統合。また、XAML と呼ばれる XML ファイルで GUI アプリケーションの視覚デザインができる。

* Windows Communication Foundation（<strong id="wcf" class="keyword">WCF</strong>）… Web アプリケーション用のフレームワーク。通信プロトコルの差異を吸収するのが目的で、多様な通信プロトコルに対して一貫した方法で開発可能。

* Windows Workflow Foundation（<strong id="wf" class="keyword">WF</strong>）… ワークフロー（業務や処理の流れ）用のフレームワーク。プログラミング言語で書くよりもフローチャートや状態遷移図で書く方が楽なような、大まかなレベルのワークフローを、ビジュアルデザイナで作ったり、XOML と呼ばれる XML ファイルで書いたりできる。

* Windows CardSpace（<strong id="wcs" class="keyword">WCS</strong>）… ユーザの ID 管理用フレームワーク。


ここでは、WPF や WCF などの説明をしていきます。

* 「[Windows Presentation Foundation](../index.md#wpf)」

* 「[Windows Communication Foundation](../index.md#wcf)」



## <a id="sec-generated-title-3"></a> <a id="dotnet3_5"></a>.NET Framework 3.5

.NET Framework 3.5 では、LINQ（Language Integrated Query）と呼ばれる機能が追加されました。
.NET Framework 3.0 がライブラリの追加のみだったのに対して、3.5 では C# も 3.0 にバージョンアップしました。
CLI（実行インフラ）自体は 2.0 のままです。

* C# 3.0、VB 9 … 言語内に SQL 文風のデータ操作が書ける「[クエリ式](../../csharp/data/sp3_linq.md#query)」という構文が追加されました。

* LINQ … 色々なデータ集合に対して、さまざまな操作を同じ形式のメソッド呼び出しでできるようにするための規約。
    * LINQ to Object … 配列や<code>IEnumerable&lt;T&gt;</code>に対して、データ操作を行う。

    * LINQ to SQL … SQL Server に格納されたデータに対して、SQL 文を自動生成することでデータ操作を行う。




詳しくは「[LINQ](../../csharp/data/sp3_linq.md)」参照。


## <a id="sec-generated-title-4"></a> <a id="dotnet4"></a>.NET Framework 4.0

.NET Framework 4.0 では非常に多岐にわたるライブラリの追加が行われています。
また、CLI も、仕様こそ 2.0 から変更はされていないものの、
パフォーマンス的な理由で1から作り直したとのことで、バージョンが変わっています。
C# や VB のバージョンもそれぞれ 4.0、10 になりました。

.NET Framwork 4.0 で追加されたライブラリの中で、主要なものをいくつか挙げます。

* TPL（Task Parallel Library: タスク並列ライブラリ） … タスクスケジューリングによる並列処理用ライブラリ。

* DLR（Dynamic Language Runtime） … 動的言語のための実行基盤。動的言語に限らず、動的コード生成もサポート。


その他、WPF などの既存のライブラリにも大幅な機能追加が行われています。
