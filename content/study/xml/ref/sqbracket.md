---
title: "[]括弧"
source_url: "https://ufcpp.net/study/xml/ref/sqbracket/"
content_type: "Article"
published_at: "2015-05-06T14:25:35"
updated_at: "2015-05-06T14:25:35"
tags: []
umbraco_id: 1692
parent_id: 1661
sort_order: 30
aliases:
  - "/ref/sqbracket"
  - "/ref/sqbracket.html"
  - "/study/ref/sqbracket"
  - "/study/ref/sqbracket.html"
  - "/xml/ref/sqbracket/"
---

# \[\]括弧

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

[]の表示
（obsolete。bracket に移行。）


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<sqbracket size="括弧の大きさ">括弧内の式</sqbracket>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<sqbracket>H,G</sqbracket> = HG &#x2212; GH
```
<div class="math"><span class="paren" style="font-size:em;">[</span>H,G<span class="paren" style="font-size:em;">]</span> = HG − GH
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:sqbracket">
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size"/>em;</xsl:attribute>
    [
  </span>
  <xsl:apply-templates/>
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size"/>em;</xsl:attribute>
    ]
  </span>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
span.paren
{
  font-style:normal;
  vertical-align:middle;
}
```
