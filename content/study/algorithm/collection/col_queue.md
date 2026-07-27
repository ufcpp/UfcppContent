---
title: "待ち行列"
source_url: "https://ufcpp.net/study/algorithm/collection/col_queue/"
content_type: "Article"
published_at: "2015-05-06T14:05:16"
updated_at: "2015-07-13T13:34:21"
tags: []
umbraco_id: 1138
parent_id: 1128
sort_order: 9
aliases:
  - "/study/algorithm/col_queue.html"
---

# 待ち行列

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<strong id="queue" class="keyword">待ち行列</strong>（queue）とは、
図1に示すように、最初に挿入した要素から順に取り出す（first in first out）ようなデータ構造です。
first in first out の頭文字からとって、FIFO バッファと呼んだりもします。

<figure>

[![待ち行列](../../../../assets/media/ufcpp2000/algorithm/fig/col_queue0.png)](../../../../assets/media/ufcpp2000/algorithm/fig/col_queue0.png)

<figcaption>待ち行列</figcaption>
</figure>


名前どおり、順番待ちで列を作って並んでいるようなイメージのデータ構造です。
待ち行列に要素を挿入することをエンキュー（enqueue: 列に入る）、
削除することをデキュー（dequeue: 列を出る）といいます。

ただし、コレクションの種類によって要素の挿入・削除の呼び名が異なるのが嫌で、
待ち行列に対しても Push/Pop という名前で挿入・削除を行うような実装方法もよく行われます。


## <a id="sec-generated-title-2"></a> <a id="impl"></a>実装方法

待ち行列は、コレクションの先頭および末尾の両方に対して要素の挿入・削除を行います。
したがって、待ち行列の実装には、
「[循環バッファ](col_circular.md#circular)」や「[双方向連結リスト](col_blist.md#blist)」を使います。
これらのコレクションは、先頭および末尾への要素の挿入・削除が高速に行えます。


## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプルソース

C# サンプルソースを示します。
「[循環バッファ](col_circular.md#circular)」を使った実装です。

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/Queue.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/Queue.cs)
