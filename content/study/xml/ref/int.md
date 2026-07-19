---
title: "積分記号"
source_url: "https://ufcpp.net/study/xml/ref/int/"
content_type: "Article"
published_at: "2015-05-06T14:25:03"
updated_at: "2015-05-06T14:25:03"
tags: []
umbraco_id: 1678
parent_id: 1661
sort_order: 16
aliases:
  - "/ref/int"
  - "/ref/int.html"
  - "/study/ref/int"
  - "/study/ref/int.html"
  - "/xml/ref/int/"
---

# 積分記号

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

積分記号を表示する


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<int><sub>積分記号の下に来る文字</sub><sup>積分記号の上に来る文字</sup></int>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<int><sub>a</sub><sup>b</sup></int> f(x) <d/>x
```
<div class="math"><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table> f(x) <span class="normal">d</span>x
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:int">
  <span class="integral">∫</span>
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
