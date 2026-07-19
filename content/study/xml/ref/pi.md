---
title: "Π"
source_url: "https://ufcpp.net/study/xml/ref/pi/"
content_type: "Article"
published_at: "2015-05-06T14:25:26"
updated_at: "2015-05-06T14:25:26"
tags: []
umbraco_id: 1687
parent_id: 1661
sort_order: 25
aliases:
  - "/ref/Pi"
  - "/ref/Pi.html"
  - "/study/ref/Pi"
  - "/study/ref/Pi.html"
  - "/xml/ref/pi/"
---

# Π

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

積の記号Πを表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<Pi><sub>Πの下にくる式</sub><sup>Πの上にくる式</sup></Pi>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
n<factorial/> = <Pi><sub>k=0</sub><sup>n</sup></Pi>k
```
<div class="math">n<span class="normal">!</span> = <table class="sigma" summary="product"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k=0</td></tr></table>k
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:Pi">
  <table class="sigma" summary="product">
    <tr><td class="sigmasub"><xsl:apply-templates select="ufcpp:sup"/></td></tr>
    <tr><td class="sigma">&#8719;</td></tr>
    <tr><td class="sigmasub"><xsl:apply-templates select="ufcpp:sub"/></td></tr>
  </table>
</xsl:template>

<xsl:template match="ufcpp:Sigma/ufcpp:sup|ufcpp:Pi/ufcpp:sup|ufcpp:Sigma/ufcpp:sub|ufcpp:Pi/ufcpp:sub">
  <xsl:apply-templates/>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
table.sigma
{
  display:inline;
  text-align:center;
  vertical-align:middle;
  font-style:italic;
}

td.sigma
{
  font-style:normal;
  font-size:120%;
}

td.sigmasub
{
  font-size:70%;
}
```
