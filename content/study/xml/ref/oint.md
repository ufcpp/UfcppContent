---
title: "周回積分記号"
source_url: "https://ufcpp.net/study/xml/ref/oint/"
content_type: "Article"
published_at: "2015-05-06T14:25:11"
updated_at: "2015-05-06T14:25:11"
tags: []
umbraco_id: 1682
parent_id: 1661
sort_order: 20
aliases:
  - "/ref/oint"
  - "/ref/oint.html"
  - "/study/ref/oint"
  - "/study/ref/oint.html"
  - "/xml/ref/oint/"
---

# 周回積分記号

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

周回積分記号を表示する


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<oint><sub>積分記号の下に来る文字</sub><sup>積分記号の上に来る文字</sup></oint>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<oint><sub>C</sub></oint> <vec>f(x)</vec>・<dl/>
```
<div class="math"><span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table> <span class="vector">f(x)</span>・<span class="normal">d</span><span class="vector">l</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:oint">
  <span class="ointegral">∮</span>
  <table class="integral" summary="integral">
    <tr><td class="intsup">&#xA0;<xsl:apply-templates select="ufcpp:sup"/></td></tr>
    <tr><td style="font-size:30%;">&#xA0;</td></tr>
    <tr><td class="intsub"><xsl:apply-templates select="ufcpp:sub"/></td></tr>
  </table>
</xsl:template>

<xsl:template match="ufcpp:oint/ufcpp:sup|ufcpp:int/ufcpp:sup|ufcpp:doubleint/ufcpp:sup|ufcpp:tripleint/ufcpp:sup">
  <xsl:apply-templates/>
</xsl:template>

<xsl:template match="ufcpp:oint/ufcpp:sub|ufcpp:int/ufcpp:sub|ufcpp:doubleint/ufcpp:sub|ufcpp:tripleint/ufcpp:sub">
  <xsl:apply-templates/>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
span.integral
{
  font-size:140%;
  font-style:normal;
  vertical-align:middle;
  margin-right:-0.1em;
}
span.ointegral
{
  font-size:140%;
  font-style:normal;
  vertical-align:middle;
  margin-right:-0.4em;
}

table.integral
{
  display:inline;
  vertical-align:middle;
  font-size:80%;
  font-style:italic;
  padding-right:0.3em;
  padding-left:0.1em;
}

td.intsup
{
  text-align:right;
  margin:0;
  padding:0;
}

table.integral td.intsub
{
  text-align:left;
  margin:0;
  padding:0;
}
```
