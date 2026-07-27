---
title: "微分"
source_url: "https://ufcpp.net/study/xml/ref/differential/"
content_type: "Article"
published_at: "2015-05-06T14:24:50"
updated_at: "2015-05-06T14:24:50"
tags: []
umbraco_id: 1672
parent_id: 1661
sort_order: 10
aliases:
  - "/study/ref/differential.html"
---

# 微分

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

d/dx や ∂/∂x 等を表示する


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<dd var="x" func="y"/> <pdd var="x" func="y/>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<dd var="s"/>f
＝
<pdd var="x" func="f"/><d/>x
＋
<pdd var="y" func="f"/><d/>y
＋
<pdd var="y" func="f"/><d/>x
```
<div class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span></td></tr></table>f
＝
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table><span class="normal">d</span>x
＋
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table><span class="normal">d</span>y
＋
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table><span class="normal">d</span>x
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:dd">
  <table class="frac" summary="differential">
    <tr><td class="num"><span class="normal">d</span><xsl:value-of select="@func"/></td></tr>
    <tr><td><span class="normal">d</span>
    <xsl:choose>
      <xsl:when test="@var != ''">
        <xsl:value-of select="@var"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates/>
      </xsl:otherwise>
    </xsl:choose>
    </td></tr>
  </table>
</xsl:template>
<xsl:template match="ufcpp:pdd">
  <table class="frac" summary="differential">
    <tr><td class="num">��<xsl:value-of select="@func"/></td></tr>
    <tr><td>��
    <xsl:choose>
      <xsl:when test="@var != ''">
        <xsl:value-of select="@var"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates/>
      </xsl:otherwise>
    </xsl:choose>
    </td></tr>
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
