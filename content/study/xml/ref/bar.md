---
title: "バー"
source_url: "https://ufcpp.net/study/xml/ref/bar/"
content_type: "Article"
published_at: "2015-05-06T14:24:37"
updated_at: "2015-05-06T14:24:37"
tags: []
umbraco_id: 1665
parent_id: 1661
sort_order: 3
aliases:
  - "/ref/bar"
  - "/ref/bar.html"
  - "/study/ref/bar"
  - "/study/ref/bar.html"
  - "/xml/ref/bar/"
---

# バー

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

変数を上線付きにする


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<bar>上線つきにしたい式</bar>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
a XOR b = a<bar>b</bar>+<bar>a</bar>b
```
<div class="math">a XOR b = a<span class="bar">b</span>+<span class="bar">a</span>b
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:bar">
  <span class="bar">
    <xsl:apply-templates/>
  </span>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
span.bar
{
  display:inline-block;
  border-top:1pt solid #000000;
}
```
