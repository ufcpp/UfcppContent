---
title: "縦ベクトル"
source_url: "https://ufcpp.net/study/xml/ref/vervec/"
content_type: "Article"
published_at: "2015-05-06T14:25:47"
updated_at: "2015-05-06T14:25:47"
tags: []
umbraco_id: 1698
parent_id: 1661
sort_order: 36
aliases:
  - "/ref/vervec"
  - "/ref/vervec.html"
  - "/study/ref/vervec"
  - "/study/ref/vervec.html"
  - "/xml/ref/vervec/"
---

# 縦ベクトル

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

縦ベクトルを表示する


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<vervec size="ベクトルの要素数"><elem>要素1</elem>...<elem>要素n<elem></vervec>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<vec>r</vec> = <vervec size="2"><elem>x</elem><elem>y</elem></vervec>
```
<div class="math"><span class="vector">r</span> = <span class="paren" style="font-size:2em;">[</span><table class="matrix" summary="vector"><tr><td>x</td></tr><tr><td>y</td></tr></table><span class="paren" style="font-size:2em;">]</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:vervec">
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size"/>em;</xsl:attribute>
    [
  </span>
  <table class="matrix" summary="vector">
    <xsl:apply-templates select="ufcpp:elem"/>
  </table>
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size" />em;</xsl:attribute>
    ]
  </span>
</xsl:template>

<xsl:template match="ufcpp:vervec/ufcpp:elem">
  <tr><td><xsl:apply-templates/></td></tr>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
table.matrix
{
  display:inline;
  font-style:italic;
  text-align:center;
  vertical-align:bottom;
  vertical-align:middle;
}

span.paren
{
  font-style:normal;
  vertical-align:middle;
}
```
