---
title: "フーリエ変換など"
source_url: "https://ufcpp.net/study/xml/ref/fourier/"
content_type: "Article"
published_at: "2015-05-06T14:24:58"
updated_at: "2015-05-06T14:24:58"
tags: []
umbraco_id: 1676
parent_id: 1661
sort_order: 14
aliases:
  - "/study/ref/Fourier.html"
---

# フーリエ変換など

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

フーリエ変換、ラプラス変換、Z変換の記号。


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<Fourier>変換元</Fourier>
<Laplace>変換元</Laplace>
<Z>変換元</Z>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<Fourier>f<paren>t</paren></Fourier><paren>ω</paren>, 
<Laplace>f<paren>t</paren></Laplace><paren>s</paren>, 
<Z>f<paren>t</paren></Z><paren>z</paren>
```
<div class="math"><span class="normal">ℱ</span><span class="paren" style="font-size:em;">[</span>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span>,
<span class="normal">ℒ</span><span class="paren" style="font-size:em;">[</span>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>, 
<span class="script">Z</span><span class="paren" style="font-size:em;">[</span>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:Fourier">
  <span class="normal">&#x2131;</span>
  <xsl:if test="@inv!=''">
  <sup>�|1</sup>
  </xsl:if>
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

<xsl:template match="ufcpp:Laplace">
  <span class="normal">&#x2112;</span>
  <xsl:if test="@inv!=''">
  <sup>�|1</sup>
  </xsl:if>
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

<xsl:template match="ufcpp:Z">
  <span class="script">
    Z
  </span>
  <xsl:if test="@inv!=''">
  <sup>�|1</sup>
  </xsl:if>
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
span.cursive
{
  font-family:cursive;
  font-style:italic;
  padding-right:0.2em;
}

span.paren
{
  font-style:normal;
  vertical-align:middle;
}
```
