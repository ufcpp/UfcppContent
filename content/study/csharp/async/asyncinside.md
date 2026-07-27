---
title: "非同期処理を支えるインフラ"
source_url: "https://ufcpp.net/study/csharp/async/asyncinside/"
content_type: "Article"
published_at: "2014-02-23T00:00:00"
updated_at: "2015-05-06T14:12:10"
tags: []
umbraco_id: 1329
parent_id: 1326
sort_order: 2
aliases:
  - "/study/csharp/AsyncInside.html"
---

# 非同期処理を支えるインフラ

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

前節の「[非同期処理の種類](asyncvariation.md)」では、非同期処理の種類と、それらを C# でどう書くか（どういうライブラリを使うか）について説明しました。
そこで出てきた、非同期メソッド、<code>Parallel</code> クラス、並列 LINQ、TPL Dataflow などの言語機能・ライブラリは、
すべて内部的に <code>Task</code> クラス（<code>System.Threading.Tasks</code> 名前空間）というクラスを使っています。

その Task クラスについて説明


##### <a id="sec-generated-title-2"></a>※

本稿の内容は[プログラミングの魔導書 Vol. 3](http://longgate.co.jp/books/grimoire-vol3.html)に寄稿したものがベースとなっています（分割、体裁、章構成の変更）。


## <a id="sec-generated-title-3"></a> <a id="task-class"></a>Task クラス

魔導書、非同期処理の内部-&gt;Taskクラス
http://ufcpp.net/study/csharp/misc_task.html これも統合
http://www.atmarkit.co.jp/ait/articles/1109/30/news126.html これの ■タスク も


## <a id="sec-generated-title-4"></a> <a id="thread-pool"></a>スレッド プール

魔導書、非同期処理の内部-&gt;スレッドプール


### <a id="sec-generated-title-5"></a> <a id="work-stealing-queue"></a>ワーク スティーリング キュー

魔導書、非同期処理の内部-&gt;ワークスティーリングキュー


## <a id="sec-generated-title-6"></a> <a id="io-completion"></a>I/O 待ち

魔導書、非同期処理の内部-&gt;非同期I/O
http://csharptan.wordpress.com/2011/12/10/%e9%9d%9e%e5%90%8c%e6%9c%9fio%e5%be%85%e3%81%a1/
こっちの方が詳しい
