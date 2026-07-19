---
title: "書体"
source_url: "https://ufcpp.net/study/xml/ref/font/"
content_type: "Article"
published_at: "2015-05-06T14:24:55"
updated_at: "2015-05-06T14:24:55"
tags: []
umbraco_id: 1675
parent_id: 1661
sort_order: 13
aliases:
  - "/ref/font"
  - "/ref/font.html"
  - "/study/ref/font"
  - "/study/ref/font.html"
  - "/xml/ref/font/"
---

# 書体

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

通常の文字列（text）、
太字（bold）、
筆記体（cursive）


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<text>通常の文字列</text> <bold>太字</bold> <cursive>筆記体</cursive>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
α ∈ <bold>C</bold>,
<cursive>Re</cursive><paren>α</paren> ∈ <bold>R</bold>
<text>（C や R は太字、Re は筆記体で書く。）</text>
```
<div class="math">α ∈ <span class="bold">C</span>,
<span class="cursive">Re</span><span class="paren" style="font-size:em;">(</span>α<span class="paren" style="font-size:em;">)</span> ∈ <span class="bold">R</span>
<span class="normal">（C や R は太字、Re は筆記体で書く。）</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:text">
  <span class="normal">
  <xsl:apply-templates/>
  </span>
</xsl:template>

<xsl:template match="ufcpp:bold">
  <span class="bold"><xsl:apply-templates/></span>
</xsl:template>

<xsl:template match="ufcpp:cursive">
  <span class="cursive"><xsl:apply-templates/></span>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
span.bold
{
  font-weight:bold;
  font-style:normal;
}
```
