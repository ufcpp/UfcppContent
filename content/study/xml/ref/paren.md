---
title: "()括弧"
source_url: "https://ufcpp.net/study/xml/ref/paren/"
content_type: "Article"
published_at: "2015-05-06T14:25:16"
updated_at: "2015-05-06T14:25:16"
tags: []
umbraco_id: 1684
parent_id: 1661
sort_order: 22
aliases:
  - "/study/ref/paren.html"
---

# ()括弧

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

()の表示
obsolete


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<paren size="括弧の大きさ">括弧内の式</paren>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
f<sup>(n)</sup> = <paren size="2"><ddt/></paren><sup>n</sup>f
```
<div class="math">f<sup>(n)</sup> = <span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup>n</sup>f
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:paren">
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size"/>em;</xsl:attribute>
    (
  </span>
  <xsl:apply-templates/>
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size"/>em;</xsl:attribute>
    )
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
