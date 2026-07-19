---
title: "条件分岐"
source_url: "https://ufcpp.net/study/xml/ref/branch/"
content_type: "Article"
published_at: "2015-05-06T14:24:42"
updated_at: "2015-05-06T14:24:42"
tags: []
umbraco_id: 1668
parent_id: 1661
sort_order: 6
aliases:
  - "/ref/branch"
  - "/ref/branch.html"
  - "/study/ref/branch"
  - "/study/ref/branch.html"
  - "/xml/ref/branch/"
---

# 条件分岐

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

条件によって値の違う式を表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<branch size="条件の数">
  <case><equ>式1</equ><cond>条件1</cond></case>
  <case><equ>式2</equ><cond>条件2</cond></case>
  .
  .
  .
  <case><equ>式n</equ><cond>条件n</cond></case>
</branch>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<abs>x</abs>=
<branch size="2">
  <case><equ>x</equ><cond><math>x≧0</math></cond></case>
  <case><equ>&#x2212;x</equ><cond><math>x&lt;0</math></cond></case>
</branch>
```
<div class="math"><span class="normal">|</span>x<span class="normal">|</span>=
<span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">x</span>  </td><td><span class="paren">(</span><span class="math"><span class="math">x≧0</span></span><span class="paren">)</span></td></tr><tr><td><span class="math">−x</span>  </td><td><span class="paren">(</span><span class="math"><span class="math">x&lt;0</span></span><span class="paren">)</span></td></tr></table>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:branch">
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size"/>em;</xsl:attribute>
    {
  </span>
  <table class="branch" summary="conditional">
    <xsl:for-each select="ufcpp:case">
      <tr>
        <td><span class="math"><xsl:apply-templates select="ufcpp:equ"/></span>&#xA0;&#xA0;</td>
        <td>
          <span class="paren">(</span>
          <span class="math"><xsl:apply-templates select="ufcpp:cond"/></span>
          <span class="paren">)</span>
        </td>
      </tr>
    </xsl:for-each>
  </table>
</xsl:template>

<xsl:template match="ufcpp:branch/ufcpp:case/ufcpp:equ">
  <xsl:apply-templates/>
</xsl:template>

<xsl:template match="ufcpp:branch/ufcpp:case/ufcpp:cond">
  <xsl:apply-templates/>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
table.branch
{
  display:inline;
  font-style:italic;
  vertical-align:bottom;
  vertical-align:middle;
}

span.paren
{
  font-style:normal;
  vertical-align:middle;
}

span.normal
{
  font-weight:normal;
  font-style:normal;
}
```
