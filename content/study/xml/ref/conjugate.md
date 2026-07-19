---
title: "複素共役"
source_url: "https://ufcpp.net/study/xml/ref/conjugate/"
content_type: "Article"
published_at: "2015-05-06T14:24:44"
updated_at: "2015-05-06T14:24:44"
tags: []
umbraco_id: 1669
parent_id: 1661
sort_order: 7
aliases:
  - "/ref/conjugate"
  - "/ref/conjugate.html"
  - "/study/ref/conjugate"
  - "/study/ref/conjugate.html"
  - "/xml/ref/conjugate/"
---

# 複素共役

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

共役複素数(右上に*を付ける)


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<conjugate>共役にしたい式</conjugate>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
z=a+ib, <conjugate>z</conjugate>=a&#x2212;ib
```
<div class="math">z=a+ib, z<sup>*</sup>=a−ib
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:conjugate">
  <xsl:apply-templates/><sup>*</sup>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```text

```
