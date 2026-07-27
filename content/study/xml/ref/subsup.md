---
title: "上付き・下付き文字"
source_url: "https://ufcpp.net/study/xml/ref/subsup/"
content_type: "Article"
published_at: "2015-05-06T14:25:37"
updated_at: "2015-05-06T14:25:37"
tags: []
umbraco_id: 1693
parent_id: 1661
sort_order: 31
aliases:
  - "/study/ref/subsup.html"
---

# 上付き・下付き文字

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

上付き・下付き文字を表示する。


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<sup>上付き</sup>
<sub>下付き</sub>
<subsup><sub>下</sub><sup>上</sup></subsup>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
x<sup>2</sup>, 
a<sub>0</sub> , 
p<subsup><sub>1</sub><sup>2</sup></subsup>
```
<div class="math">x<sup>2</sup>,
a<sub>0</sub> , 
p<table class="subsup" summary="sub / sup"><tr><td>2</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td>1</td></tr></table>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:sup0"><sup><span class="normal">0</span></sup></xsl:template>
<xsl:template match="ufcpp:sup1"><sup><span class="normal">1</span></sup></xsl:template>
<xsl:template match="ufcpp:sup2"><sup><span class="normal">2</span></sup></xsl:template>
<xsl:template match="ufcpp:sup2"><sup><span class="normal">2</span></sup></xsl:template>
<xsl:template match="ufcpp:sup3"><sup><span class="normal">3</span></sup></xsl:template>
<xsl:template match="ufcpp:sup4"><sup><span class="normal">4</span></sup></xsl:template>
<xsl:template match="ufcpp:sup5"><sup><span class="normal">5</span></sup></xsl:template>
<xsl:template match="ufcpp:sup6"><sup><span class="normal">6</span></sup></xsl:template>
<xsl:template match="ufcpp:sup7"><sup><span class="normal">7</span></sup></xsl:template>
<xsl:template match="ufcpp:sup8"><sup><span class="normal">8</span></sup></xsl:template>
<xsl:template match="ufcpp:sup9"><sup><span class="normal">9</span></sup></xsl:template>
<xsl:template match="ufcpp:sup10"><sup><span class="normal">10</span></sup></xsl:template>

<xsl:template match="ufcpp:sub0"><sub><span class="normal">0</span></sub></xsl:template>
<xsl:template match="ufcpp:sub1"><sub><span class="normal">1</span></sub></xsl:template>
<xsl:template match="ufcpp:sub2"><sub><span class="normal">2</span></sub></xsl:template>
<xsl:template match="ufcpp:sub2"><sub><span class="normal">2</span></sub></xsl:template>
<xsl:template match="ufcpp:sub3"><sub><span class="normal">3</span></sub></xsl:template>
<xsl:template match="ufcpp:sub4"><sub><span class="normal">4</span></sub></xsl:template>
<xsl:template match="ufcpp:sub5"><sub><span class="normal">5</span></sub></xsl:template>
<xsl:template match="ufcpp:sub6"><sub><span class="normal">6</span></sub></xsl:template>
<xsl:template match="ufcpp:sub7"><sub><span class="normal">7</span></sub></xsl:template>
<xsl:template match="ufcpp:sub8"><sub><span class="normal">8</span></sub></xsl:template>
<xsl:template match="ufcpp:sub9"><sub><span class="normal">9</span></sub></xsl:template>
<xsl:template match="ufcpp:sub10"><sub><span class="normal">10</span></sub></xsl:template>

<xsl:template match="ufcpp:supinv">
  <sup><span class="normal">&#8722;1</span></sup>
</xsl:template>

<xsl:template match="ufcpp:subsup">
  <table class="subsup" summary="sub / sup">
    <tr><td><xsl:apply-templates select="ufcpp:sup"/></td></tr>
    <tr><td style="font-size:30%;">&#xA0;</td></tr>
    <tr><td><xsl:apply-templates select="ufcpp:sub"/></td></tr>
  </table>
</xsl:template>

<xsl:template match="ufcpp:subsup/ufcpp:sub|ufcpp:subsup/ufcpp:sup">
  <xsl:apply-templates/>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
table.subsup
{
  display:inline;
  vertical-align:middle;
  font-size:80%;
  font-style:italic;
  padding-left:1em;
}
```
