---
title: "三角関数"
source_url: "https://ufcpp.net/study/xml/ref/sin/"
content_type: "Article"
published_at: "2015-05-06T14:25:34"
updated_at: "2015-05-06T14:25:34"
tags: []
umbraco_id: 1691
parent_id: 1661
sort_order: 29
aliases:
  - "/ref/sin"
  - "/ref/sin.html"
  - "/study/ref/sin"
  - "/study/ref/sin.html"
  - "/xml/ref/sin/"
---

# 三角関数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

sin,cos,tanの表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<sin/> <cos/> <tan/>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<sin/>x = <cos/>(x-<frac><num>π</num><denom>2</denom></frac>),
<tan/>x = <frac><num><sin/>x</num><denom><cos/>x</denom></frac>
```
<div class="math"><span class="normal">sin</span>x = <span class="normal">cos</span>(x-<table class="frac" summary="fraction"><tr><td class="num">π</td></tr><tr><td>2</td></tr></table>),
<span class="normal">tan</span>x = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">sin</span>x</td></tr><tr><td><span class="normal">cos</span>x</td></tr></table>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:sin">
  <span class="normal">sin</span>
</xsl:template>

<xsl:template match="ufcpp:cos">
  <span class="normal">cos</span>
</xsl:template>

<xsl:template match="ufcpp:tan">
  <span class="normal">tan</span>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
span.normal
{
  font-weight:normal;
  font-style:normal;
}
```
