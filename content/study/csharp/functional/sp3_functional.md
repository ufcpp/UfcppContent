---
title: "関数型言語・動的言語的な機能"
source_url: "https://ufcpp.net/study/csharp/functional/sp3_functional/"
content_type: "Article"
published_at: "2007-09-01T00:00:00"
updated_at: "2008-08-15T00:00:00"
tags:
  - "Ver. 3.0"
umbraco_id: 1283
parent_id: 1275
sort_order: 9
aliases:
  - "/study/csharp/sp3_functional.html"
---

# 関数型言語・動的言語的な機能

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version3">Ver. 3.0</h5>

C# 3.0 の新機能には、関数型言語や動的言語が由来と思われる機能がいくつかあります。
ただし、C# の方向性としては、「関数型・動的言語になる」ではなくて、
「<em>関数型・動的言語との融合</em>」です。

すなわち、C# が関数型・動的言語になったわけではなくて、
あくまで、関数型・動的言語の機能の中から手続き型・静的言語を基本とする C# でも実現できそうなものを輸入したという感じです。

具体的には、型の推論やラムダ式がそれにあたります。
これらの機能は、
「[クエリ式](../data/sp3_linq.md#query)」のために導入されたと思われる節が強いです。
それ以外の場面で使っても便利は便利なんですが、
メリットだけでなく多少の副作用もあったりするので利用の際には少し注意が必要です。

分割・移転：

* 「[型推論と匿名型](../start/sp3_inference.md)」

* 「[ラムダ式](sp3_lambda.md)」
