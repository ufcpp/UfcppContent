---
title: "C# とは"
source_url: "https://ufcpp.net/study/csharp/abstract/ab_csharp/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2007-03-31T00:00:00"
tags: []
umbraco_id: 1186
parent_id: 1185
sort_order: 0
aliases:
  - "/csharp/ab_csharp"
  - "/csharp/ab_csharp.html"
  - "/csharp/abstract/ab_csharp/"
  - "/study/csharp/ab_csharp"
  - "/study/csharp/ab_csharp.html"
---

# C# とは

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
市販アプリケーションやビジネスソフトウェアの開発現場で
最も広く使われてきている言語は C と C++ でした（90年代の話）。
この二つの言語は非常に細かなコントロールを行える柔軟な言語ですが、
柔軟性と引き換えにソフトウェア開発が難しいという欠点があります。
C や C++ を使用してアプリケーションを開発するには Visual Basic などの言語を使用して
同等のものを作る作業に比べ、開発期間が長くなります。

登場とともに一躍脚光を浴びた言語に Java があります。
Java は C++ を元に改良を加えた言語で、マルチプラットフォーム上で動作し、
GUI プログラムを作成するライブラリが標準で用意されています。
しかし、C++ と比べて、制限されたことが多く、今ひとつ物足りない仕様になっていますし、
低レベルのコードをコントロールする機能に乏しいため、
既存のシステムとの相互運用が難しく、
マルチプラットフォームで動作するといっても、
高速に動作させることのできるプラットフォームは限られています。

C や C++ プログラマは、基礎となるプラットフォームの機能にアクセスできるパワーを持ち、
高速なソフトウェアの開発が行え、
既存のソフトウェアのと統合も可能で、
さらに、必要があれば低レベルのコーディングもできる環境を望んでいます。

このようなことから、Microsoft は
<strong id="cs" class="keyword">C#</strong>(「しーしゃーぷ」と読む)と呼ばれる、新しい言語を作成しました。
C# は C++ のパワーと柔軟性を持ち、
同時に Visual Basic などの生産性の高さも持ち合わせる言語です。


##<a id="sec-generated-title-2"></a> <a id="future"></a>1.0 → 2.0 → 3.0 ...
一番初め、世に出た直後の C#、すなわち C# 1.0 は、
既存言語のいいとこ取りのようなプログラミング言語でした。
新しすぎる物を作っても利用者が付いてくれるわけもなく、
これは当然といえば当然の出だしです。

これが今では、2004年頃に C# 2.0（β版が2004年5月に発表、正式版は2004年末）、
2007年に C# 3.0（β版は2005年9月発表）とバージョンアップし、
既存言語の焼き直しに留まらない新しい試みが取り入れられています。

参考：
「[C# 2.0 の新機能](../cheatsheet/ap_ver2.md)」、
「[C# 3.0 の新機能](../cheatsheet/ap_ver3.md)」、
「[C# 4.0 の新機能](../cheatsheet/ap_ver4.md)」、
「[C# 5.0 の新機能](../cheatsheet/ap_ver5.md)」。


##<a id="sec-generated-title-3"></a> <a id="embedded"></a>組み込み開発
長らくの間、Windows といえばパソコン用 OS で、
Microsoft 主導で Windows アプリケーション開発用に作られた C#（.NET Framework）もまた、
パソコン用（特にビジネスアプリケーション開発用）なイメージがありました。

ところが近年（2005年頃から）では、
Microsoft も組み込みシステム向け製品開発にも力を入れていて、
.NET Framework を用いた組み込み開発も可能になっています。

一口に組み込みシステム向けと言っても、
要求（据え置きか携帯型かとか、筐体サイズや搭載可能な CPU など）に応じて様々な製品が用意されています。

* [Windows XP Embedded](http://www.microsoft.com/japan/windows/embedded/eval/xpe/default.mspx): 据え置き型・大型筐体（高性能）向け組み込み OS。 普通に .NET Framework が利用可能。

* [Windows CE](http://www.microsoft.com/japan/windows/embedded/eval/wince/default.mspx)/[Windows Mobile](http://www.microsoft.com/japan/windowsmobile/default.mspx): 携帯型・小型機器向け組み込み OS。[.NET Compact Framework](http://www.microsoft.com/japan/msdn/vstudio/device/netcf/)という、.NET Framework のサブセットが利用可能。 （ただし、Managed コードの実行にはハードリアルタイム動作の保証なし。）

* [.NET Micro Framework](http://msdn2.microsoft.com/en-us/embedded/bb267253.aspx): Windows CE も載せられないくらいのローエンド・低消費電力システム向けに、 （Windows CE などの OS 機能抜きで）.NET Framework のサブセットを提供。

* [XNA](http://www.microsoft.com/japan/msdn/directx/xna/default.aspx): Xbox 360 用 / Windows 用ゲーム開発プラットフォーム。 C# を用いたゲーム開発が容易に行えます。
