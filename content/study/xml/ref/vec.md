---
title: "ベクトル"
source_url: "https://ufcpp.net/study/xml/ref/vec/"
content_type: "Article"
published_at: "2015-05-06T14:25:46"
updated_at: "2015-05-06T14:25:46"
tags: []
umbraco_id: 1697
parent_id: 1661
sort_order: 35
aliases:
  - "/ref/vec"
  - "/ref/vec.html"
  - "/study/ref/vec"
  - "/study/ref/vec.html"
  - "/xml/ref/vec/"
---

# ベクトル

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

太字イタリック体のベクトル


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<vec>ベクトルにしたい文字</vec>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<vec>y</vec> = A<vec>x</vec>
```
<div class="math"><span class="vector">y</span> = A<span class="vector">x</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:vec">
  <span class="vector"><xsl:apply-templates/></span>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
span.vector
{
  font-weight:bold;
}
```
