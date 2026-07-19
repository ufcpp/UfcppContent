---
title: "スタック"
source_url: "https://ufcpp.net/study/algorithm/collection/col_stack/"
content_type: "Article"
published_at: "2015-05-06T14:05:13"
updated_at: "2015-07-13T13:33:58"
tags: []
umbraco_id: 1137
parent_id: 1128
sort_order: 8
aliases:
  - "/algorithm/col_stack"
  - "/algorithm/col_stack.html"
  - "/algorithm/collection/col_stack/"
  - "/study/algorithm/col_stack"
  - "/study/algorithm/col_stack.html"
---

# スタック

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
<strong id="stack" class="keyword">スタック</strong>（stack: 積み重ね、堆積）とは、
図1に示すように、最後に挿入した要素から順に取り出す（first in last out）ようなデータ構造です。
first in last out の頭文字からとって、FILO バッファと読んだりもします。

<figure>
	[![スタック](../../../../assets/media/ufcpp2000/algorithm/fig/col_stack0.png)](../../../../assets/media/ufcpp2000/algorithm/fig/col_stack0.png)
	<figcaption>スタック</figcaption>
</figure>


スタックに値を挿入することをプッシュ（push）、
取り出すことをポップ（pop）するといいます。
日本語の場合、プッシュは“積む”といったりもします。
“積む”という言葉通り、
荷物を上に載せていくようなイメージです。
上に積んだ荷物を先にどけないと、下の荷物が取りだせません。


##<a id="sec-generated-title-2"></a> <a id="impl"></a>実装方法
スタックは、コレクションの先頭あるいは末尾のどちらか一方に対してのみ要素の挿入・削除を行います。
したがって、スタックの実装には、
「[配列リスト](col_array.md#array)」や「[片方向連結リスト](col_flist.md#flist)」を使います。
これらのコレクションは、先頭あるいは末尾への要素の挿入・削除が高速に行えます。


##<a id="sec-generated-title-3"></a> <a id="sample"></a>サンプルソース
C# サンプルソースを示します。
「[配列リスト](col_array.md#array)」を使った実装です。

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/Stack.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/Stack.cs)
