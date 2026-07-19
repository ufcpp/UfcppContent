---
title: "その他の用語"
source_url: "https://ufcpp.net/study/csharp/appendix/ap_term/"
content_type: "Article"
published_at: "2008-03-14T00:00:00"
updated_at: "2019-06-09T17:52:02"
tags: []
umbraco_id: 1378
parent_id: 1377
sort_order: 0
aliases:
  - "/csharp/ap_term"
  - "/csharp/ap_term.html"
  - "/csharp/appendix/ap_term/"
  - "/study/csharp/ap_term"
  - "/study/csharp/ap_term.html"
---

# その他の用語

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

どのページに書くにも収まりの悪かったものをここに。


## <a id="sec-generated-title-2"></a> <a id="ducktype"></a>ダック タイピング

C# では通常、クラスやインターフェースの型情報に基づいてメソッド呼び出しが行われます。
一方で、動的言語と呼ばれるような言語では、
「同じ名前のメソッドを持っていれば変数の型は問わない」
というような方針でメソッドを呼び出せます。

動的言語でよく見られるような、インターフェースに頼るのではなく、
メソッドなどの名前だけ見て処理を振り分けるようなプログラミングスタイルを
「<strong id="ducktype" class="keyword">ダック タイピング</strong>（duck typing）」といいます。
（「アヒルのように歩き、アヒルのように鳴くものはアヒルに違いない」という格言が由来で、
「見てくれ一緒なら同じ扱いしてもいいじゃない」という意味。）

C# 4.0 以降では、[`dynamic`](../dynamic/sp4_dynamic.md) キーワードを使うことでダックタイピングが可能です。

また、インターフェイスの実装が不要という意味では、[パターン ベース](../misc/miscpatternbased.md)な構文(例えば [foreach](../data/sp_foreach.md#foreach) や[クエリ式](../data/sp3_linq.md#query))のこともダック タイピングということがありました。
ただ、ダック タイピングという言葉は動的な処理で好まれるものなので、
最近ではパターン ベースな構文を指して使うことはなくなってきました。
