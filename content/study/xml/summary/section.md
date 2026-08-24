---
title: "章の参照"
source_url: "https://ufcpp.net/study/xml/summary/section/"
content_type: "Article"
published_at: "2015-05-06T14:24:16"
updated_at: "2015-07-07T18:21:46"
tags: []
umbraco_id: 1654
parent_id: 1650
sort_order: 3
aliases:
  - "/study/testxsl/section.html"
---

# 章の参照

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

[<code>section.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/section.xsl) には、章の表示（サブサブセクションまで階層的に表示）や、
章の参照、ドキュメントへのリンクのための template が記述されています。


## <a id="sec-generated-title-2"></a> <a id="source"></a>ソース

```xml {title="ソース"}
<section title="結果" id="result">
  <p>
    section タグでセクションを作ります。
  </p>
  <section title="サブセクション1" id="sub1">
    <p>
      section タグの中に section タグを入れ子にすると、
      サブセクションになります。
    </p>
  </section>
  <section title="サブセクション2" id="sub2">
    <p>
      サブセクション2
    </p>
    <p>
      ref タグで、他のセクションを参照することが出来ます。
      例 → <ref doc="section" id="result" />。
    </p>
    <p>
      また、link タグで、他のドキュメントへのリンクを作れます。
      例 → <link doc="document" />。
    </p>
    <section title="サブサブセクション" id="subsub">
      <p>
        さらに入れ子にすることで、サブサブセクションまで表示できます。
      </p>
    </section>
  </section>
</section>
```
id 属性を省略すると、ランダムな ID を割り当ててくれる機能もありますが、非推奨です。
（[XSD](../../../../assets/media/ufcpp2000/xsd/xsd.zip) では id 属性を必須属性にしてあります。）


## <a id="sec-generated-title-3"></a> <a id="result"></a>結果

section タグでセクションを作ります。


### <a id="sec-generated-title-4"></a> <a id="sub1"></a>サブセクション1

section タグの中に section タグを入れ子にすると、
サブセクションになります。


### <a id="sec-generated-title-5"></a> <a id="sub2"></a>サブセクション2

サブセクション2

ref タグで、他のセクションを参照することが出来ます。
例 → 「[結果](#result)」。

また、link タグで、他のドキュメントへのリンクを作れます。
例 → 「[ドキュメント](document.md)」。


#### <a id="sec-generated-title-6"></a> <a id="subsub"></a>サブサブセクション

さらに入れ子にすることで、サブサブセクションまで表示できます。
