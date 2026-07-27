---
title: ".NET とは"
source_url: "https://ufcpp.net/study/csharp/abstract/ab_dotnet/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2008-01-12T00:00:00"
tags: []
umbraco_id: 1187
parent_id: 1185
sort_order: 1
aliases:
  - "/study/csharp/ab_dotnet.html"
---

# .NET とは

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# は「.NET 上で動く言語」（正確には Common Language Infrastructure (CLI) 規格上で動く言語）です。
（規格上は、C# と .NET は独立していますが、実質的には一体的に開発されています。）

.NET は、Microsoft が Windows 向けに作った .NET Framework という開発プラットフォームが起源です。
その後、Linux や Mac OS などを含むクロスプラットフォーム向けに再開発され、こちらは .NET Core と名づけられました。
これらを総称して(あるいは統合して) .NET と呼びます。

また、(.NET Framework の発表からほどなくして、 .NET Core より前から)
.NET Framework 互換プラットフォームとして Mono というプロジェクトがあります。
Mono は現在では iOS や Android 上でも動いていて、[Xamarin](https://docs.microsoft.com/ja-jp/xamarin/) や [Unity](https://unity.com/)の根幹となっています。

2019年現在、Mono と .NET Core の統合作業が行われています。
統合された暁にはこれを指して「.NET」と呼ばれる予定です。

## <a id="sec-generated-title-2"></a> <a id="framework"></a>.NET Framework

Microsoft <strong id="dotnet" class="keyword">.NET Framework</strong> とは Microsft が提供する、（2000年6月に発表された）開発プラットフォームです。

.NET Framework では、プログラムはネイティブコードではなく、<strong id="il" class="keyword">IL</strong> (Intermediate Language)と呼ばれる中間コードにコンパイルされます。
この IL を実行するための環境（の仕様）を <strong id="cli" class="keyword">CLI</strong> (Common Language Infrastructure)といいます。
IL は、この CLI 仕様にそった実装さえされていれば任意のプラットフォームで実行することが出来ます。

また、IL は CLI によってファイル、メモリ、ネットワーク接続などのリソースを管理されていて、
メモリの解放し忘れなどによるリソースリークを防ぐことが出来ます。

.NET Framework に対応したすべての言語は IL にコンパイルされます。
現在、.NET Framework に対応している言語は Microsoft が提供している C#、VB.NET、F#、C++/CLI (C++を.NETに適応するように改良したもの) や、
サードパーティの提供する Perl や Python など、20種類以上のものがあります。
.NET Framework では、これらすべての言語で同じライブラリを利用できますし、
異なる言語で書かれたプログラムを呼び出すことが出来ます。
例えば、C++/CLI で書かれたクラスを継承した新しいクラスを VB.NET で作り、そのクラスを C# から呼び出すといったことも可能です。

また、.NET Framework を用いることでプログラマは COM やレジストリなどの知識がなくても分散アプリケーションの作成、配布が容易に行えます。

VB、JScript、C++ などの既存の言語は、.NET Framework に対応出来るように拡張されています。
一方、<strong id="csharp" class="keyword">C#</strong> は初めから .NET Framework 上で使うことを想定して設計された言語です。

そのほかにも .NET Framework はさまざまな特徴を備えています。
表1に、.NET Framework の特徴を簡単にまとめます。
詳細については
[MSDN](http://www.microsoft.com/japan/developer/net/framework/dnmag_framework.asp)
をご覧ください。

<table summary=".NET Framework の特徴">
	<caption>
		.NET Framework の特徴
	</caption>
	<tr>
		<td markdown="1">言語の統合（共通型システム）</td>
		<td markdown="1">C++、VB、C#など複数の言語を統合する。例えば、VBで作成したクラスの派生クラスをC++で作成することも可能である。</td>
	</tr>
	<tr>
		<td markdown="1">プログラミングの簡素化</td>
		<td markdown="1">COMやレジストリの知識を必要としない。</td>
	</tr>
	<tr>
		<td markdown="1">さまざまなプラットフォームでの実行</td>
		<td markdown="1">Common Language Infrastructure (CLI)をサポートする任意のプラットフォームで実行することが出来る。将来的にはWindows以外のプラットフォーム用のCLRも作成される可能性がある。</td>
	</tr>
	<tr>
		<td markdown="1">自動リソース管理</td>
		<td markdown="1">ファイル、メモリ、ネットワーク接続などのリソースの利用状況を自動的に追跡し、アプリケーションによるリソースリークを防ぐ。</td>
	</tr>
	<tr>
		<td markdown="1">充実したライブラリ/フレームワーク</td>
		<td markdown="1">ウェブ サーバー、デスクトップ クライアント、データベース アクセス、ゲーム等々、 さまざまな種類のプログラムを作るためのフレームワークが整っています。</td>
	</tr>
</table>

## <a id="sec-generated-title-3"></a> <a id="netcore"></a>.NET Core

2014年頃から Microsoft はオープンソース化の動きを見せるようになりました。
今では、GitHub 最大のコミット数を誇る会社になっています。

その流れに沿って、これまで(Microsoft 自身では) Windows でしか提供してこなかった .NET を Linux や Mac OS でも動くように、
オープンでクロスプラットフォームに作り直すことになり、.NET Core と名づけました。

(ちなみに、当時も Mono は Linux でも Mac OS でも動いていましたが、
当時の情勢としてはモバイル(iOS/Android)に注力していて、
Linux/Mac OSでの安定性・パフォーマンスはそれほど良くなく、
.NET Core は Mono とは別に新たに作り直しています。
ただし、以前から .NET Framework の基本ライブラリのソースコードはオープンソース化されつつあり、
Mono も徐々に .NET Framework のソースコードを取り込みつつある状況だったので、まったくの別物というわけではありません。)

2016年6月の .NET Core 1.0 リリース時点では「最低限動くもの」という感じで、
標準提供されているライブラリは .NET Framework のサブセットでした。
しかし、2019年9月リリースの .NET Core 3.0 では(Windows でしか動かないものはありますが)主要なものが一通り移植され、
新規開発では .NET Framework ではなく .NET Core を使うことが推奨されています。

## <a id="sec-generated-title-4"></a> <a id="dotnet"></a>.NET

前述の通り、作り直しのために一度は分岐した .NET Framework と .NET Core ですが、
現在は再統合の流れ(というより、.NET Core を推奨して、.NET Framework は保守モード)になっています。
また、.NET Core と Mono の統合も進められています。

.NET Framework は 4.8 が最終バージョンとなります。
.NET Core に一本化されるため、.NET Core 3.1 の次のバージョンからは「.NET」という呼称に変わります。

ちなみに、.NET Framework 4.X との混乱を避けて、 
.NET Core 3.1 の次は .NET 5 となる予定です。

## <a id="sec-generated-title-5"></a> <a id="version"></a>.NET Framework のバージョン

移転: 「[バージョン](../cheatsheet/list_versions.md)」
