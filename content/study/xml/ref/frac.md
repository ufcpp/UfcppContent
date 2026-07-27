---
title: "分数"
source_url: "https://ufcpp.net/study/xml/ref/frac/"
content_type: "Article"
published_at: "2015-05-06T14:25:00"
updated_at: "2015-05-06T14:25:00"
tags: []
umbraco_id: 1677
parent_id: 1661
sort_order: 15
aliases:
  - "/study/ref/frac.html"
---

# 分数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

分数を表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<frac>
  <num>分子</num>
  <denom>分母</denom>
</frac>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<frac><num>f(b)&#x2212;f(a)</num><denom>b&#x2212;a</denom></frac> = f'(c)
```
<div class="math"><table class="frac" summary="fraction"><tr><td class="num">f(b)−f(a)</td></tr><tr><td>b−a</td></tr></table> = f'(c)
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:frac">

  <table class="frac" summary="fraction">
    <tr><td class="num"><xsl:choose><xsl:when test="@num != ''"><xsl:value-of select="@num"/></xsl:when><xsl:when test="@n != ''"><xsl:value-of select="@n"/></xsl:when><xsl:otherwise><xsl:apply-templates select="ufcpp:num"/></xsl:otherwise></xsl:choose></td></tr>
    <tr><td><xsl:choose><xsl:when test="@denom != ''"><xsl:value-of select="@denom"/></xsl:when><xsl:when test="@d != ''"><xsl:value-of select="@d"/></xsl:when><xsl:otherwise><xsl:apply-templates select="ufcpp:denom"/></xsl:otherwise></xsl:choose></td></tr>
  </table>
</xsl:template>

<xsl:template match="ufcpp:frac/ufcpp:num">
  <xsl:apply-templates/>
</xsl:template>

<xsl:template match="ufcpp:frac/ufcpp:denom">
  <xsl:apply-templates/>
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
```
