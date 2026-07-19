---
title: "{}括弧"
source_url: "https://ufcpp.net/study/xml/ref/brace/"
content_type: "Article"
published_at: "2015-05-06T14:24:39"
updated_at: "2015-05-06T14:24:39"
tags: []
umbraco_id: 1666
parent_id: 1661
sort_order: 4
aliases:
  - "/ref/brace"
  - "/ref/brace.html"
  - "/study/ref/brace"
  - "/study/ref/brace.html"
  - "/xml/ref/brace/"
---

# {}括弧

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

{}の表示。
（obsolete。bracket に移行。）


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<brace size="括弧の大きさ">括弧内の式</brace>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<brace size="1.2"><paren>x-ξ</paren><sup>2</sup>+<paren>y-η</paren><sup>2</sup></brace><sup>1/2</sup>
```
<div class="math"><span class="paren" style="font-size:1.2em;">{</span><span class="paren" style="font-size:em;">(</span>x-ξ<span class="paren" style="font-size:em;">)</span><sup>2</sup>+<span class="paren" style="font-size:em;">(</span>y-η<span class="paren" style="font-size:em;">)</span><sup>2</sup><span class="paren" style="font-size:1.2em;">}</span><sup>1/2</sup>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:brace">
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size"/>em;</xsl:attribute>
    {
  </span>
  <xsl:apply-templates/>
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size"/>em;</xsl:attribute>
    }
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
