---
title: "行列"
source_url: "https://ufcpp.net/study/xml/ref/matrix/"
content_type: "Article"
published_at: "2015-05-06T14:25:09"
updated_at: "2015-05-06T14:25:09"
tags: []
umbraco_id: 1681
parent_id: 1661
sort_order: 19
aliases:
  - "/study/ref/matrix.html"
---

# 行列

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

テーブル状の行列を表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<matrix size="行列の高さ">
  <row><elem>1,1成分</elem>...<elem>1,n成分</elem>
  .
  .
  .
  <row><elem>m,1成分</elem>...<elem>m,n成分</elem>
</matrix>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<vervec size="3"><elem>x'</elem><elem>y'</elem><elem>z'</elem></vervec> = 
<matrix size="3">
<row><elem>a</elem><elem>b</elem><elem>c</elem></row>
<row><elem>d</elem><elem>e</elem><elem>f</elem></row>
<row><elem>g</elem><elem>h</elem><elem>i</elem></row>
</matrix>
<vervec size="3"><elem>x</elem><elem>y</elem><elem>z</elem></vervec>
```
<div class="math"><span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="vector"><tr><td>x'</td></tr><tr><td>y'</td></tr><tr><td>z'</td></tr></table><span class="paren" style="font-size:3em;">]</span> =
<span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>a</td><td>b</td><td>c</td></tr><tr><td>d</td><td>e</td><td>f</td></tr><tr><td>g</td><td>h</td><td>i</td></tr></table><span class="paren" style="font-size:3em;">]</span>
<span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="vector"><tr><td>x</td></tr><tr><td>y</td></tr><tr><td>z</td></tr></table><span class="paren" style="font-size:3em;">]</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:matrix">
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size"/>em;</xsl:attribute>
    [
  </span>
  <table class="matrix" summary="matrix">
    <xsl:apply-templates select="ufcpp:row"/>
  </table>
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size" />em;</xsl:attribute>
    ]
  </span>
</xsl:template>

<xsl:template match="ufcpp:array">
  <table class="matrix" summary="array">
    <xsl:apply-templates select="ufcpp:row"/>
  </table>
</xsl:template>

<xsl:template match="ufcpp:matrix/ufcpp:row|ufcpp:array/ufcpp:row">
  <tr><xsl:apply-templates select="ufcpp:elem"/></tr>
</xsl:template>

<xsl:template match="ufcpp:matrix/ufcpp:row/ufcpp:elem|ufcpp:array/ufcpp:row/ufcpp:elem">
  <td><xsl:apply-templates/></td>
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
