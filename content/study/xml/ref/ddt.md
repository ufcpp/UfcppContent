---
title: "時間微分"
source_url: "https://ufcpp.net/study/xml/ref/ddt/"
content_type: "Article"
published_at: "2015-05-06T14:24:48"
updated_at: "2015-05-06T14:24:48"
tags: []
umbraco_id: 1671
parent_id: 1661
sort_order: 9
aliases:
  - "/study/ref/ddt.html"
---

# 時間微分

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

時間微分記号d/dtを表示する


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<ddt/>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<ddt/>x = -kx
```
<div class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>x = -kx
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:ddt">
  <table class="frac" summary="differential">
    <tr><td class="num"><span class="normal">d</span><xsl:choose><xsl:when test="@var != ''"><xsl:value-of select="@var"/></xsl:when><xsl:when test="@v != ''"><xsl:value-of select="@v"/></xsl:when><xsl:otherwise><xsl:apply-templates/></xsl:otherwise></xsl:choose></td></tr>
    <tr><td><span class="normal">d</span>t</td></tr>
  </table>
</xsl:template>

<xsl:template match="ufcpp:math//ufcpp:dd|ufcpp:Math//ufcpp:dd">
  <table class="frac" summary="differential">
    <tr><td class="num"><span class="normal">d</span><xsl:choose><xsl:when test="@num != ''"><xsl:value-of select="@num"/></xsl:when><xsl:when test="@n != ''"><xsl:value-of select="@n"/></xsl:when><xsl:otherwise><xsl:apply-templates select="ufcpp:num"/></xsl:otherwise></xsl:choose></td></tr>
    <tr><td><span class="normal">d</span><xsl:choose><xsl:when test="@denom != ''"><xsl:value-of select="@denom"/></xsl:when><xsl:when test="@d != ''"><xsl:value-of select="@d"/></xsl:when><xsl:otherwise><xsl:apply-templates select="ufcpp:denom"/></xsl:otherwise></xsl:choose></td></tr>
  </table>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
table.frac
{
  display:inline;
  vertical-align:middle;
  font-style:italic;
  font-size:90%;
  text-align:center;
}

td.num
{
  border-bottom:#000000 1pt solid;
}

span.normal
{
  font-weight:normal;
  font-style:normal;
}
```
