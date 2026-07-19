---
title: "アレフ"
source_url: "https://ufcpp.net/study/xml/ref/aleph/"
content_type: "Article"
published_at: "2015-05-06T14:24:33"
updated_at: "2015-05-06T14:24:33"
tags: []
umbraco_id: 1663
parent_id: 1661
sort_order: 1
aliases:
  - "/ref/aleph"
  - "/ref/aleph.html"
  - "/study/ref/aleph"
  - "/study/ref/aleph.html"
  - "/xml/ref/aleph/"
---

# アレフ

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

無限基数アレフを表示。


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<aleph sub="subscript"/>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<aleph/>, 
<aleph sub="1"/> ＝ 2<sup><aleph sub="0"/></sup>
```
<div class="math">‭א,
‭א<sub>1</sub> ＝ 2<sup>‭א<sub>0</sub></sup>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:aleph">
  &#x202D;&#x05D0;
  <xsl:if test="@sub!=''">
    <sub><xsl:value-of select="@sub"/></sub>
  </xsl:if>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```text

```
